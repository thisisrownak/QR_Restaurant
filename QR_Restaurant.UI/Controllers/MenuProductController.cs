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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QR_Restaurant.UI.Controllers
{
    [Authorize(Roles = "RestaurantAdmin, Admin")]
    public class MenuProductController : Controller
    {
        private IMenuProductService _menuProductService;
        private IMenuCategoryService _menuCategoryService;
        private UserManager<AppUser> _userManager;
        private LocService _locService;
        private IWebHostEnvironment _hostEnvironment;

        public MenuProductController(IMenuProductService menuProductService, UserManager<AppUser> userManager, LocService locService,
            IMenuCategoryService menuCategoryService, IWebHostEnvironment hostEnvironment)
        {
            _menuProductService = menuProductService;
            _userManager = userManager;
            _locService = locService;
            _menuCategoryService = menuCategoryService;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            AppUser User = _userManager.Users.Where(x => x.UserName == HttpContext.User.Identity.Name)
               .Include(x => x.Restaurant)
               .SingleOrDefault();
            MenuProductViewModel model = new MenuProductViewModel()
            {
                MenuProducts = _menuProductService.GetAllByRestaurant((int)User.RestaurantId),
                RestaurantCurrencySymbol = User.Restaurant.CurrencySymbol
            };
            return View(model);
        }

        public IActionResult AddOrUpdate(int id = 0)
        {
            AppUser User = _userManager.Users.SingleOrDefault(x => x.UserName == HttpContext.User.Identity.Name);
            MenuProductViewModel model = new MenuProductViewModel()
            {
                Id = 0,
                MenuCategories = _menuCategoryService.GetAllByRestaurant((int)User.RestaurantId)
            };
            if (id != 0)
            {
                MenuProduct entity = _menuProductService.GetWithCategoryById(id);
                if (entity == null)
                {
                    return View("Error");
                }

                if (entity.MenuCategory.RestaurantId != User.RestaurantId)
                {
                    return View("Error");
                }
                model.Id = entity.Id;
                model.Name = entity.Name;
                model.Description = entity.Description;
                model.Price = entity.Price;
                model.RowNumber = entity.RowNumber;
                model.PhotoUrl = entity.PhotoUrl;
                model.MenuCategoryId = entity.MenuCategoryId;
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult AddOrUpdate(MenuProductViewModel model)
        {
            AppUser User = _userManager.Users.SingleOrDefault(x => x.UserName == HttpContext.User.Identity.Name);
            if (!ModelState.IsValid)
            {
                model.MenuCategories = _menuCategoryService.GetAllByRestaurant((int)User.RestaurantId);
                return View(model);
            }

            if (model.Id == 0)
            {
                MenuCategory menuCategory = _menuCategoryService.GetMenuCategory(model.MenuCategoryId);
                if (menuCategory == null || (menuCategory != null && menuCategory.RestaurantId != User.RestaurantId))
                {
                    return View("Error");
                }
                MenuProduct entity = new MenuProduct()
                {
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    PhotoUrl = UploadPhoto(model.Photo),
                    RowNumber = model.RowNumber,
                    MenuCategoryId = model.MenuCategoryId
                };

                _menuProductService.Add(entity);
                TempData.Add("messagex", String.Format($"{_locService.GetLocalizedValue("SuccessProcess")}*success"));
            }
            else
            {
                MenuProduct entity = _menuProductService.GetWithCategoryById(model.Id);
                if (entity == null)
                {
                    return View("Error");
                }

                if (entity.MenuCategory.RestaurantId != User.RestaurantId)
                {
                    return View("Error");
                }

                entity.UpdatedDate = DateTime.Now;
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Price = model.Price;
                entity.RowNumber = model.RowNumber;
                entity.PhotoUrl = model.Photo != null ? UploadPhoto(model.Photo, entity.PhotoUrl) : entity.PhotoUrl;   //Save Photo method**
                model.MenuCategoryId = entity.MenuCategoryId;

                _menuProductService.Update(entity);
                TempData.Add("messagex", String.Format($"{_locService.GetLocalizedValue("SuccessProcess")}*success"));
            };

            return RedirectToAction("Index");
        }

        public IActionResult Passive(int id = 0)
        {
            MenuProduct entity = _menuProductService.GetWithCategoryById(id);

            if (entity == null)
            {
                return View("Error");
            }
            AppUser User = _userManager.Users.SingleOrDefault(x => x.UserName == HttpContext.User.Identity.Name);
            if (entity.MenuCategory.RestaurantId != User.RestaurantId)
            {
                return View("Error");
            }

            entity.IsActive = false;
            entity.UpdatedDate = DateTime.Now;
            _menuProductService.Update(entity);

            TempData.Add("messagex", String.Format($"{_locService.GetLocalizedValue("SuccessProcess")}*success"));
            return RedirectToAction("Index");
        }

        public IActionResult Activated(int id = 0)
        {
            MenuProduct entity = _menuProductService.GetWithCategoryById(id);

            if (entity == null)
            {
                return View("Error");
            }
            AppUser User = _userManager.Users.SingleOrDefault(x => x.UserName == HttpContext.User.Identity.Name);
            if (entity.MenuCategory.RestaurantId != User.RestaurantId)
            {
                return View("Error");
            }

            entity.IsActive = true;
            entity.UpdatedDate = DateTime.Now;
            _menuProductService.Update(entity);

            TempData.Add("messagex", String.Format($"{_locService.GetLocalizedValue("SuccessProcess")}*success"));
            return RedirectToAction("Index");
        }

        private string UploadPhoto(IFormFile file, string oldPhoto = "")
        {
            if (file != null)
            {
                string returnUrl = "";
                Guid newName = Guid.NewGuid();
                string extension = Path.GetExtension(file.FileName);

                var path = Path.Combine(
                      Directory.GetCurrentDirectory(), "wwwroot/images/menuProduct",
                      newName + extension);


                returnUrl = newName + extension;

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                if (!String.IsNullOrEmpty(oldPhoto))
                {
                    string fullPath = Path.Combine(_hostEnvironment.WebRootPath, $"images\\menuProduct\\{oldPhoto}");
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

        [HttpGet]
        public IActionResult GetProductByCategory(int id)
        {
            return Json(_menuProductService.GetAllByCategory(id));
        }
    }
}
