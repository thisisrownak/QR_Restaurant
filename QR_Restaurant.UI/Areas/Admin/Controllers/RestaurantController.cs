using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QR_Restaurant.Business.Abstract;
using QR_Restaurant.Data.Entities;
using QR_Restaurant.UI.Areas.Admin.Models;
using QR_Restaurant.UI.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QR_Restaurant.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RestaurantController : Controller
    {
        private IRestaurantService _restaurantSerivce;
        private UserManager<AppUser> _userManager;
        private LocService _locService;

        public RestaurantController(IRestaurantService restaurantSerivce, UserManager<AppUser> userManager, LocService locService)
        {
            _restaurantSerivce = restaurantSerivce;
            _userManager = userManager;
            _locService = locService;
        }
        public IActionResult Index()
        {
            IEnumerable<Restaurant> entities = _restaurantSerivce.GetAll();
            return View(entities);
        }

        public IActionResult Passive(int id = 0)
        {
            Restaurant entity = _restaurantSerivce.GetRestaurant(id);

            if (entity == null)
            {
                return View("Error");
            }

            entity.IsActive = false;
            entity.UpdatedDate = DateTime.Now;
            entity.LastBlockDate = DateTime.Now;
            _restaurantSerivce.Update(entity);

            TempData.Add("messagex", String.Format($"{_locService.GetLocalizedValue("SuccessProcess")}*success"));
            return Redirect("/admin/restaurant/Index");
        }

        public IActionResult Activated(int id = 0)
        {
            Restaurant entity = _restaurantSerivce.GetRestaurant(id);

            if (entity == null)
            {
                return View("Error");
            }

            entity.IsActive = true;
            entity.UpdatedDate = DateTime.Now;
            _restaurantSerivce.Update(entity);

            TempData.Add("messagex", String.Format($"{_locService.GetLocalizedValue("SuccessProcess")}*success"));
            return Redirect("/admin/restaurant/Index");
        }

        public JsonResult GetRestaurantStaffs(int restaurantId = 0)
        {
            List<StaffsModel> Staffs = _userManager.Users.Where(x => x.RestaurantId == restaurantId).ToList().Select(x => new StaffsModel()
            {
                FullName = $"{x.Name} {x.Surname}",
                Role = _userManager.GetRolesAsync(x).Result.SingleOrDefault() == "RestaurantAdmin" ? _locService.GetLocalizedValue("Manager"): _locService.GetLocalizedValue("Waiter"),
                UserName = x.UserName                
            }).ToList();
            return Json(Staffs);
        }
    }
}
