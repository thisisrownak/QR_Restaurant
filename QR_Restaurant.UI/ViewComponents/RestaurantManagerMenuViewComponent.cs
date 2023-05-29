using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;
using QR_Restaurant.Data.Entities;
using QR_Restaurant.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QR_Restaurant.UI.ViewComponents
{
    public class RestaurantManagerMenuViewComponent : ViewComponent
    {
        protected readonly UserManager<AppUser> _userManager;

        public RestaurantManagerMenuViewComponent(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public ViewViewComponentResult Invoke()
        {
            var User = _userManager.Users.Where(x => x.UserName == HttpContext.User.Identity.Name)
           .Include(x => x.Restaurant)
           .SingleOrDefault();

            RestaurantSettingsViewModel model = new RestaurantSettingsViewModel()
            {
                Name = User.Restaurant.Name,
                Phone = User.Restaurant.Phone,
                LogoUrl = User.Restaurant.LogoUrl,
                Description = User.Restaurant.Description,
                Address =User.Restaurant.Address
            };

            return View(model);
        }
    }
}
