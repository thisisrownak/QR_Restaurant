using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QR_Restaurant.Business.Abstract;
using QR_Restaurant.Data.Entities;
using QR_Restaurant.UI.Helper;
using QR_Restaurant.UI.Models;
using QR_Restaurant.UI.SingalR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QR_Restaurant.UI.Controllers
{
    public class RestaurantController : Controller
    {
        private UserManager<AppUser> _userManager;
        private IRestaurantService _restaurantService;
        private IMenuCategoryService _menuCategoryService;
        private IQrOrderTableService _tableService;
        private IOrderService _orderService;
        private IWebHostEnvironment _hostEnvironment;
        private LocService _locService;
        private OrderHub _socketService;

        public RestaurantController(UserManager<AppUser> userManager, IRestaurantService restaurantService, IWebHostEnvironment hostEnvironment,
            LocService locService, IMenuCategoryService menuCategoryService, IQrOrderTableService tableService, IOrderService orderService,
            OrderHub socketService)
        {
            _userManager = userManager;
            _restaurantService = restaurantService;
            _menuCategoryService = menuCategoryService;
            _tableService = tableService;
            _hostEnvironment = hostEnvironment;
            _locService = locService;
            _orderService = orderService;
            _socketService = socketService;
        }

        [Authorize(Roles = "RestaurantAdmin, Admin, Waiter")]
        public IActionResult RestaurantManager()
        {
            var User = _userManager.Users.SingleOrDefault(x => x.UserName == HttpContext.User.Identity.Name);

            if (User != null)
            {
                RestaurantManagerViewModel model = new RestaurantManagerViewModel()
                {
                    Id = User.RestaurantId,
                    Tables = _tableService.GetQrTablesWithActiveOrders((int)User.RestaurantId)
                };

                return View(model);
            }

            return View();
        }

        //Other RestaurantManager Page for order monitor
        [Authorize(Roles = "RestaurantAdmin, Admin, Waiter")]
        public IActionResult Orders()
        {
            var User = _userManager.Users
                .Where(x => x.UserName == HttpContext.User.Identity.Name)
                .Include(x => x.Restaurant)
                .SingleOrDefault();
            if (User != null)
            {
                RestaurantManagerViewModel model = new RestaurantManagerViewModel()
                {
                    Id = User.RestaurantId,
                    CurrencySymbol = User.Restaurant.CurrencySymbol
                };

                return View(model);
            }

            return View();
        }

        //Other RestaurantManager Page for order board monitor
        [Authorize(Roles = "RestaurantAdmin, Admin, Waiter")]
        public IActionResult OrderBoard()
        {
            var User = _userManager.Users.SingleOrDefault(x => x.UserName == HttpContext.User.Identity.Name);

            if (User != null)
            {
                RestaurantManagerViewModel model = new RestaurantManagerViewModel()
                {
                    Id = User.RestaurantId
                };

                return View(model);
            }

            return View();
        }

        [Authorize(Roles = "RestaurantAdmin, Admin, Waiter")]
        public IActionResult OrdersOfTable(int id)
        {
            var table = _tableService.GetQrOrderTable(id);

            if (table == null)
            {
                return View("Error");
            }

            var User = _userManager.Users
                .Where(x => x.UserName == HttpContext.User.Identity.Name)
                .Include(x => x.Restaurant)
                .SingleOrDefault();

            var Staff = _userManager.Users.Where(x => x.RestaurantId == User.RestaurantId && x.Id != User.Id && x.EmailConfirmed).Select(x => new StaffComplateModel()
            {
                UserId = x.Id,
                Avatar = x.Avatar
            }).ToList();

            var orders = _orderService.GetActiveOrdersByTable(table.Id).Select(x => new RestaurantOrderJsonModel()
            {
                Id = x.Id,
                TableName = x.QrOrderTable.Name,
                TableNo = x.QrOrderTable.TableNo,
                Status = _locService.GetLocalizedValue(x.OrderStatus.ToString()),
                Note = x.Note,
                Date = x.CreatedDate.ToLongDateString() == DateTime.Now.ToLongDateString() ? x.CreatedDate.ToShortTimeString() : x.CreatedDate.ToLongDateString(),
                Products = x.Order_MenuProducts.Select(x => x.MenuProduct.Name).ToList(),
                ProductFeatures = x.Order_MenuProducts.Select(x => x.FeatureItemsList).ToList(),
                Quantities = x.Order_MenuProducts.Select(x => x.Quantity).ToList(),
                Staff = Staff,
                Total = x.Total
            });

            OrdersOfTableViewModel model = new OrdersOfTableViewModel()
            {
                Orders = orders,
                TableId = table.Id,
                TableNo = table.TableNo,
                CurrencySymbol = User.Restaurant.CurrencySymbol
            };

            if (!table.IsRead)
            {
                table.IsRead = true;
                _tableService.Update(table);
            }

            return View(model);
        }

        public IActionResult Menu(int id, int tableNo, string userId = "")
        {
            var User = _userManager.Users.Where(x => x.Id == userId)
           .Include(x => x.Restaurant)
           .SingleOrDefault();
            if (User == null)
            {
                return View("Error");
            }

            Restaurant restaurant = _restaurantService.GetRestaurant(id);
            //if (restaurant == null || User.RestaurantId != restaurant.Id)
            //{
            //    return View("Error");
            //}

            QrOrderTable qrTable = _tableService.GetQrOrderTable(tableNo);
            if (qrTable == null || qrTable.RestaurantId != restaurant.Id)
            {
                return View("Error");
            }

            RestaurantMenuViewModel model = new RestaurantMenuViewModel()
            {
                CategoriesWithProducts = _menuCategoryService.GetMenuCategoryWithProductsByRestaurant(restaurant.Id),
                Restaurant = restaurant,
                TableId = qrTable.Id
            };

            return View(model);
        }

        [Authorize(Roles = "RestaurantAdmin, Admin")]
        public IActionResult Settings()
        {
            var User = _userManager.Users.Where(x => x.UserName == HttpContext.User.Identity.Name)
          .Include(x => x.Restaurant)
          .SingleOrDefault();

            RestaurantSettingsViewModel model = new RestaurantSettingsViewModel()
            {
                Name = User.Restaurant.Name,
                Address = User.Restaurant.Address,
                Description = User.Restaurant.Description,
                Phone = User.Restaurant.Phone,
                LogoUrl = User.Restaurant.LogoUrl,
                IsShowTableName = User.Restaurant.IsShowTableName,
                IsShowTableNo = User.Restaurant.IsShowTableNo,
                CurrencySymbol = User.Restaurant.CurrencySymbol,
                RestaurantId = (int)User.RestaurantId
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "RestaurantAdmin, Admin")]
        public IActionResult Settings(RestaurantSettingsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Restaurant entity = _restaurantService.GetRestaurant(model.RestaurantId);

            entity.Name = model.Name;
            entity.Phone = model.Phone;
            entity.IsShowTableName = model.IsShowTableName;
            entity.IsShowTableNo = model.IsShowTableNo;
            entity.Address = model.Address;
            entity.Description = model.Description;
            entity.CurrencySymbol = model.CurrencySymbol;
            entity.LogoUrl = model.Logo != null ? UploadLogo(model.Logo, entity.LogoUrl) : entity.LogoUrl;
            _restaurantService.Update(entity);
            TempData.Add("messagex", String.Format($"{_locService.GetLocalizedValue("SuccessProcess")}*success"));
            return RedirectToAction("restaurantmanager");
        }

        public JsonResult CallTheWaiter(int restaurant, int table)
        {
            if (restaurant == 0) return Json("Restaurant Not Found");
            QrOrderTable TableEntity = _tableService.GetQrOrderTable(table);
            if (TableEntity == null) return Json("Table Not Found");

            _socketService.CallTheWaiter(restaurant.ToString(), TableEntity.TableNo, TableEntity.Name).Wait();
            return Json("200");
        }

        public JsonResult CallTheBill(int restaurant, int table)
        {
            if (restaurant == 0) return Json("Restaurant Not Found");
            QrOrderTable TableEntity = _tableService.GetQrOrderTable(table);
            if (TableEntity == null) return Json("Table Not Found");

            _socketService.CallTheBill(restaurant.ToString(), TableEntity.TableNo, TableEntity.Name).Wait();
            return Json("200");
        }

        private string UploadLogo(IFormFile file, string oldPhoto = "")
        {
            if (file != null)
            {
                string returnUrl = "";
                Guid newName = Guid.NewGuid();
                string extension = Path.GetExtension(file.FileName);

                var path = Path.Combine(
                      Directory.GetCurrentDirectory(), "wwwroot/images/logos",
                      newName + extension);


                returnUrl = newName + extension;

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                if (!String.IsNullOrEmpty(oldPhoto))
                {
                    string fullPath = Path.Combine(_hostEnvironment.WebRootPath, $"images\\logos\\{oldPhoto}");
                    if (System.IO.File.Exists(fullPath))
                        System.IO.File.Delete(fullPath);
                }
                return returnUrl;
            }
            else
            {
                return "default.png";
            }

        }
        //Other RestaurantManager Page for order board monitor
        [Authorize(Roles = "RestaurantAdmin, Admin, Waiter")]
        public IActionResult Dashboard()
        {
            return View();
        }

    }
}
