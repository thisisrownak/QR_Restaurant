using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QR_Restaurant.Business.Abstract;
using QR_Restaurant.Data.Entities;
using QR_Restaurant.UI.Helper;
using QR_Restaurant.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QR_Restaurant.UI.Controllers
{
    [Authorize(Roles = "RestaurantAdmin, Admin")]
    public class MenuCategoryController : Controller
    {
        private IMenuCategoryService _menuCategoryService;
        private UserManager<AppUser> _userManager;
        private LocService _locService;

        public MenuCategoryController(IMenuCategoryService menuCategoryService, LocService locService, UserManager<AppUser> userManager)
        {
            _menuCategoryService = menuCategoryService;
            _locService = locService;
            _userManager = userManager;
        }
        
        public IActionResult Index()
        {
            AppUser User = _userManager.Users.SingleOrDefault(x => x.UserName == HttpContext.User.Identity.Name);
            MenuCategoryViewModel model = new MenuCategoryViewModel()
            {
                MenuCategories = _menuCategoryService.GetAllByRestaurant((int)User.RestaurantId)
            };
            return View(model);
        }

        public IActionResult AddOrUpdate(int id = 0)
        {
            MenuCategoryViewModel model = new MenuCategoryViewModel()
            {
                Id = 0
            };
            if (id != 0)
            {
                MenuCategory entity = _menuCategoryService.GetMenuCategory(id);
                if (entity == null)
                {
                    return View("Error");
                }
                AppUser User = _userManager.Users.SingleOrDefault(x => x.UserName == HttpContext.User.Identity.Name);
                if (entity.RestaurantId != User.RestaurantId)
                {
                    return View("Error");
                }
                model.Id = entity.Id;
                model.Name = entity.Name;
                model.RowNumber = entity.RowNumber;
                model.RestaurantId = entity.RestaurantId;
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult AddOrUpdate(MenuCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            AppUser User = _userManager.Users.SingleOrDefault(x => x.UserName == HttpContext.User.Identity.Name);
            if (model.Id == 0)
            {
                MenuCategory entity = new MenuCategory()
                {
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    Name = model.Name,
                    RowNumber = model.RowNumber,
                    RestaurantId = (int)User.RestaurantId
                };

                _menuCategoryService.Add(entity);
                TempData.Add("messagex", String.Format($"{_locService.GetLocalizedValue("SuccessProcess")}*success"));
            }
            else
            {
                MenuCategory entity = _menuCategoryService.GetMenuCategory(model.Id);
                if (entity == null)
                {
                    return View("Error");
                }
                
                if (entity.RestaurantId != User.RestaurantId)
                {
                    return View("Error");
                }

                entity.UpdatedDate = DateTime.Now;
                entity.Name = model.Name;
                entity.RowNumber = model.RowNumber;

                _menuCategoryService.Update(entity);
                TempData.Add("messagex", String.Format($"{_locService.GetLocalizedValue("SuccessProcess")}*success"));
            };

            return RedirectToAction("Index");
        }

        public IActionResult Passive(int id = 0)
        {
            MenuCategory entity = _menuCategoryService.GetMenuCategory(id);

            if (entity == null)
            {
                return View("Error");
            }
            AppUser User = _userManager.Users.SingleOrDefault(x => x.UserName == HttpContext.User.Identity.Name);
            if (entity.RestaurantId != User.RestaurantId)
            {
                return View("Error");
            }

            entity.IsActive = false;
            entity.UpdatedDate = DateTime.Now;
            _menuCategoryService.Update(entity);

            TempData.Add("messagex", String.Format($"{_locService.GetLocalizedValue("SuccessProcess")}*success"));
            return RedirectToAction("Index");
        }

        public IActionResult Activated(int id = 0)
        {
            MenuCategory entity = _menuCategoryService.GetMenuCategory(id);

            if (entity == null)
            {
                return View("Error");
            }
            AppUser User = _userManager.Users.SingleOrDefault(x => x.UserName == HttpContext.User.Identity.Name);
            if (entity.RestaurantId != User.RestaurantId)
            {
                return View("Error");
            }

            entity.IsActive = true;
            entity.UpdatedDate = DateTime.Now;
            _menuCategoryService.Update(entity);

            TempData.Add("messagex", String.Format($"{_locService.GetLocalizedValue("SuccessProcess")}*success"));
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Json(_menuCategoryService.GetActiveCategory());
        }
    }
}
