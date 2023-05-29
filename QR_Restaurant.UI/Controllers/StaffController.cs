using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QR_Restaurant.Business.Abstract;
using QR_Restaurant.Data.Entities;
using QR_Restaurant.UI.Helper;
using QR_Restaurant.UI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QR_Restaurant.UI.Controllers
{
    [Authorize(Roles = "RestaurantAdmin, Admin")]
    public class StaffController : Controller
    {
        private UserManager<AppUser> _userManager;
        private IOrderService _orderService;
        private RoleManager<AppRole> _roleManager;
        private LocService _locService;

        public StaffController(UserManager<AppUser> userManager, IOrderService orderService, RoleManager<AppRole> roleManager, LocService locService)
        {
            _userManager = userManager;
            _orderService = orderService;
            _roleManager = roleManager;
            _locService = locService;

        }
        public IActionResult Index()
        {
            var User = _userManager.Users.Where(x => x.UserName == HttpContext.User.Identity.Name)
                .Include(x => x.Restaurant)
                .SingleOrDefault();

            RestaurantStaffViewModel model = new RestaurantStaffViewModel()
            {
                RestaurantId = (int)User.RestaurantId,
                Staff = _userManager.Users.Where(x => x.RestaurantId == User.RestaurantId && x.Id != User.Id)
            };

            return View(model);
        }

        public IActionResult AddStaff()
        {
            AppUser User = _userManager.Users.SingleOrDefault(x => x.UserName == HttpContext.User.Identity.Name);
            RestaurantStaffViewModel model = new RestaurantStaffViewModel()
            {
                RestaurantId = (int)User.RestaurantId
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult AddStaff(RestaurantStaffViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            AppUser user = new AppUser
            {
                UserName = model.UserName,
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Avatar = UploadAvatar(model.Photo),
                RegistrationDate = DateTime.Now,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                RestaurantId = model.RestaurantId
            };

            IdentityResult result = _userManager.CreateAsync(user, model.Password).Result;

            if (result.Succeeded)
            {
                if (!_roleManager.RoleExistsAsync("Waiter").Result)
                {
                    AppRole role = new AppRole
                    {
                        Name = "Waiter"
                    };

                    IdentityResult roleResult = _roleManager.CreateAsync(role).Result;

                    if (!roleResult.Succeeded)
                    {
                        ModelState.AddModelError("", "Role didn't add!");
                        return View(model);
                    }
                }
                _userManager.AddToRoleAsync(user, "Waiter").Wait();
            }
            TempData.Add("messagex", String.Format($"{_locService.GetLocalizedValue("SuccessProcess")}*success"));
            return Redirect("/staff");
        }

        public IActionResult PassiveStaff(string userId)
        {
            AppUser User = _userManager.Users.SingleOrDefault(x => x.Id == userId);
            if (User == null)
            {
                return View("Error");
            }

            User.EmailConfirmed = false;

            IdentityResult updateResult = _userManager.UpdateAsync(User).Result;
            if (updateResult.Succeeded)
            {
                TempData.Add("messagex", String.Format($"{_locService.GetLocalizedValue("SuccessProcess")}*success"));
                return Redirect("/Staff");
            }
            return View("Error");
        }

        public IActionResult StaffActivated(string userId)
        {
            AppUser User = _userManager.Users.SingleOrDefault(x => x.Id == userId);
            if (User == null)
            {
                return View("Error");
            }

            User.EmailConfirmed = true;

            IdentityResult updateResult = _userManager.UpdateAsync(User).Result;
            if (updateResult.Succeeded)
            {
                TempData.Add("messagex", String.Format($"{_locService.GetLocalizedValue("SuccessProcess")}*success"));
                return Redirect("/staff");
            }
            return View("Error");
        }

        public IActionResult DeliveredOrders(string userId)
        {
            var User = _userManager.Users.Where(x => x.UserName == HttpContext.User.Identity.Name)
               .SingleOrDefault();

            AppUser Waiter = _userManager.Users.SingleOrDefault(x => x.Id == userId);
            if (Waiter == null || Waiter.RestaurantId != User.RestaurantId)
            {
                return View("Error");
            }

            DeliveredOrdersViewModel model = new DeliveredOrdersViewModel()
            {
                WaiterId = userId,
                WaiterNameAndSurname = $"{Waiter.Name} {Waiter.Surname}",
                StartDate = DateTime.Now.AddDays(-1),
                EndDate = DateTime.Now
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult DeliveredOrders(DeliveredOrdersViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var User = _userManager
                .Users.Where(x => x.UserName == HttpContext.User.Identity.Name)
               .Include(x => x.Restaurant)
                .SingleOrDefault();

            AppUser Waiter = _userManager.Users.SingleOrDefault(x => x.Id == model.WaiterId);
            if (Waiter == null || Waiter.RestaurantId != User.RestaurantId)
            {
                return View("Error");
            }

            DeliveredOrdersViewModel viewModel = new DeliveredOrdersViewModel()
            {
                WaiterId = Waiter.Id,
                WaiterNameAndSurname = $"{Waiter.Name} {Waiter.Surname}",
                Orders = _orderService.GetWaiterOrders(Waiter.Id, model.StartDate, model.EndDate).Select(x => new RestaurantOrderJsonModel()
                {
                    Id = x.Id,
                    TableName = x.QrOrderTable.Name,
                    TableNo = x.QrOrderTable.TableNo,
                    Status = _locService.GetLocalizedValue(Enumeration.GetEnumDescription(x.OrderStatus)),
                    Note = x.Note,
                    Date = x.CreatedDate.ToLongDateString() == DateTime.Now.ToLongDateString() ? x.CreatedDate.ToShortTimeString() : x.CreatedDate.ToLongDateString(),
                    Products = x.Order_MenuProducts.Select(x => x.MenuProduct.Name).ToList(),
                    Quantities = x.Order_MenuProducts.Select(x => x.Quantity).ToList(),
                    Total = x.Total
                }).ToList(),
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                CurrencySymbol = User.Restaurant.CurrencySymbol
            };
            return View(viewModel);
        }

        private string UploadAvatar(IFormFile file)
        {
            if (file != null)
            {
                string returnUrl = "";
                Guid newName = Guid.NewGuid();
                string extension = Path.GetExtension(file.FileName);

                var path = Path.Combine(
                      Directory.GetCurrentDirectory(), "wwwroot/images/avatars",
                      newName + extension);


                returnUrl = newName + extension;

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return returnUrl;
            }
            else
            {
                return "default.png";
            }

        }

    }
}
