using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QR_Restaurant.Business.Abstract;
using QR_Restaurant.Data.Entities;
using QR_Restaurant.UI.Helper;
using QR_Restaurant.UI.Models;
using QRCoder;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QR_Restaurant.UI.Controllers
{
    [Authorize(Roles = "RestaurantAdmin, Admin")]
    public class QrCodeController : Controller
    {
        private IQrOrderTableService _qrOrderService;
        private UserManager<AppUser> _userManager;
        private LocService _locService;
        private readonly ValidationLocalizer _localizer;
        public QrCodeController(IQrOrderTableService qrOrderService, UserManager<AppUser> userManager, LocService locService, ValidationLocalizer localizer)
        {
            _qrOrderService = qrOrderService;
            _userManager = userManager;
            _locService = locService;
            _localizer = localizer;
        }
        public IActionResult Index()
        {
            AppUser User = _userManager.Users
                .Where(x => x.UserName == HttpContext.User.Identity.Name)
                .SingleOrDefault();

            QrCodeViewModel model = new QrCodeViewModel()
            {
                QrOrderTables = _qrOrderService.GetQrTablesWithRestaurant(Convert.ToInt32(User.RestaurantId)),
                HasError = "N",
                RestaurantId = Convert.ToInt32(User.RestaurantId)
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult CreateQrCodeOrderTable(QrCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.HasError = "Y";
                model.QrOrderTables = _qrOrderService.GetQrTablesWithRestaurant(model.RestaurantId);
                return View("Index", model);
            }

            if(_qrOrderService.GetQrOrderTableByNoOrNameForRestaurant(model.RestaurantId, model.TableNo != model.OldTableNo ? model.TableNo : null) != null)
            {
                model.HasError = "Y";
                model.QrOrderTables = _qrOrderService.GetQrTablesWithRestaurant(model.RestaurantId);
                ModelState.AddModelError("TableNo", _localizer.GetLocalizedValue("Exists"));
                return View("Index", model);
            }

            AppUser User = _userManager.Users.Where(x => x.UserName == HttpContext.User.Identity.Name)
           .SingleOrDefault();

            try
            {
                _qrOrderService.BeginTransaction();
                if (model.Id == 0)
                {
                    QrOrderTable entity = new QrOrderTable()
                    {
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        Name = model.Name,
                        RestaurantId = Convert.ToInt32(User.RestaurantId),
                        TableNo = model.TableNo,
                        IsRead = true
                    };

                    _qrOrderService.Add(entity);

                    entity.QrCodeUrl = GenerateQrCode((int)User.RestaurantId, entity.Id, User.Id);
                    _qrOrderService.Update(entity);
                }
                else
                {
                    QrOrderTable entity = _qrOrderService.GetQrOrderTable(model.Id);
                    entity.UpdatedDate = DateTime.Now;
                    entity.TableNo = model.TableNo;
                    entity.Name = model.Name;
                    entity.IsActive = model.IsActive;
                    _qrOrderService.Update(entity);
                }

                _qrOrderService.CommitTransaction();
                TempData.Add("messagex", String.Format($"{_locService.GetLocalizedValue("SuccessProcess")}*success"));
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                _qrOrderService.RollbackTransaction();
                return View("Error", e.Message);
            }
        }

        public IActionResult PrintQrCodeTemplate(int id)
        {
            QrOrderTable entity = _qrOrderService.GetQrOrderTable(id);
            QrCodeViewModel model = new QrCodeViewModel()
            {
                Name = entity.Name,
                TableNo = entity.TableNo,
                QrCodeUrl = entity.QrCodeUrl,
                IsShowTableName = entity.Restaurant.IsShowTableName,
                IsShowTableNo = entity.Restaurant.IsShowTableNo
            };

            return new ViewAsPdf(model);
        }

        public IActionResult PrintQrCodeAllTables()
        {
            AppUser User = _userManager.Users
                .Where(x => x.UserName == HttpContext.User.Identity.Name)
                .SingleOrDefault();

            QrCodeViewModel model = new QrCodeViewModel()
            {
                QrOrderTables = _qrOrderService.GetQrTablesWithRestaurant(Convert.ToInt32(User.RestaurantId))              
            };
            return new ViewAsPdf(model);
        }

        string GenerateQrCode(int id, int qrTableId, string userId)
        {
            Guid newName = Guid.NewGuid();
            string path = Path.Combine(
                     Directory.GetCurrentDirectory(), "wwwroot/images/qrCodes",
                     newName + ".png");

            using (MemoryStream ms = new MemoryStream())
            {
                QRCodeGenerator qrCodeGenerator = new QRCodeGenerator();            
                QRCodeData qrCodeData = QRCodeGenerator.GenerateQrCode($"{Request.Scheme}://{Request.Host}{Request.PathBase}/restaurant/menu?id={id}&tableNo={qrTableId}&userId={userId}", QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                using (Bitmap bitmap = qrCode.GetGraphic(20))
                {
                    //bitmap.Save(ms, ImageFormat.Png);
                    //return "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        bitmap.Save(stream, ImageFormat.Png);
                    };
                    return $"{newName}.png";
                }
            }
        }
    }
}
