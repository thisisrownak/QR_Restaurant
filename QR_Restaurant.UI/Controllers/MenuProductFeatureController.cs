using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QR_Restaurant.Business.Abstract;
using QR_Restaurant.Data.Entities;
using QR_Restaurant.UI.Helper;
using System.Collections.Generic;
using System.Data;
using System;
using QR_Restaurant.UI.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace QR_Restaurant.UI.Controllers
{
    [Authorize(Roles = "RestaurantAdmin, Admin")]
    public class MenuProductFeatureController : Controller
    {
        private IMenuProductService _menuProductService;
        private IMenuProductFeatureService _featureService;
        private IMenuProductFeatureItemService _featureItemService;
        private UserManager<AppUser> _userManager;
        private LocService _locService;

        public MenuProductFeatureController(IMenuProductService menuProductService, IMenuProductFeatureService featureService,
            IMenuProductFeatureItemService featureItemService, UserManager<AppUser> userManager, LocService locService)
        {
            _menuProductService = menuProductService;
            _featureService = featureService;
            _featureItemService = featureItemService;
            _userManager = userManager;
            _locService = locService;
        }

        public IActionResult Index()
        {
            AppUser User = _userManager.Users.SingleOrDefault(x => x.UserName == HttpContext.User.Identity.Name);
            ProductFeatureViewModel model = new ProductFeatureViewModel()
            {
                ProductFeatures = _featureService.GetFeaturesWithItemsByRestaurant((int)User.RestaurantId)
            };

            return View(model);
        }

        public IActionResult AddOrUpdate(int id = 0)
        {
            AppUser User = _userManager.Users
                .Where(x => x.UserName == HttpContext.User.Identity.Name)
                .Include(x => x.Restaurant)
                .SingleOrDefault();

            ProductFeatureViewModel model = new ProductFeatureViewModel()
            {
                Id = 0,
                MenuProducts = _menuProductService.GetAllByRestaurant((int)User.RestaurantId)
            };
            if (id != 0)
            {
                MenuProductFeature entity = _featureService.GetWithFeatureItemsById(id);
                if (entity == null)
                {
                    return View("Error");
                }

                if (entity.MenuProduct.MenuCategory.RestaurantId != User.RestaurantId)
                {
                    return View("Error");
                }
                model.Id = entity.Id;
                model.Name = entity.Name;
                model.IsMultiSelect = entity.IsMultiSelect;
                model.MenuProductId = entity.MenuProductId;
                model.FeatureItems = entity.ProductFeatureItems;
                model.RestaurantCurrencySymbol = User.Restaurant.CurrencySymbol;
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult AddOrUpdate(ProductFeatureViewModel model)
        {
            AppUser User = _userManager.Users.SingleOrDefault(x => x.UserName == HttpContext.User.Identity.Name);
            if (!ModelState.IsValid)
            {
                model.MenuProducts = _menuProductService.GetAllByRestaurant((int)User.RestaurantId);
                return View(model);
            }

            if (model.Id == 0)
            {
                MenuProduct menuProduct = _menuProductService.GetWithCategoryById(model.MenuProductId);
                if (menuProduct == null || (menuProduct != null && menuProduct.MenuCategory.RestaurantId != User.RestaurantId))
                {
                    return View("Error");
                }

                MenuProductFeature entity = new MenuProductFeature()
                {
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    Name = model.Name,
                    IsMultiSelect = model.IsMultiSelect,
                    MenuProductId = model.MenuProductId
                };

                //_featureService.BeginTransaction();
                try
                {
                    _featureService.Add(entity);

                    string[] items = model.FeatureItemList.Split("#");

                    List<MenuProductFeatureItem> entites = new List<MenuProductFeatureItem>();
                    for (int i = 0; i < items.Length; i++)
                    {
                        MenuProductFeatureItem featureItem = new MenuProductFeatureItem()
                        {
                            CreatedDate = DateTime.Now,
                            IsActive = true,
                            Name = items[i].Split("?")[0],
                            Price = Convert.ToDecimal(items[i].Split("?")[1]),
                            ProductFeatureId = entity.Id
                        };
                        entites.Add(featureItem);
                    }

                    _featureItemService.BulkAdd(entites);
                    //_featureService.CommitTransaction();
                    TempData.Add("messagex", String.Format($"{_locService.GetLocalizedValue("SuccessProcess")}*success"));
                }
                catch (Exception)
                {
                    //_featureService.RollbackTransaction();
                    return View("Error");
                }
            }
            else
            {
                MenuProductFeature entity = _featureService.GetWithFeatureItemsById(model.Id);
                if (entity == null)
                {
                    return View("Error");
                }

                if (entity.MenuProduct.MenuCategory.RestaurantId != User.RestaurantId)
                {
                    return View("Error");
                }

                entity.UpdatedDate = DateTime.Now;
                entity.Name = model.Name;
                entity.IsMultiSelect = model.IsMultiSelect;
                entity.MenuProductId = model.MenuProductId;

                _featureService.Update(entity);
                TempData.Add("messagex", String.Format($"{_locService.GetLocalizedValue("SuccessProcess")}*success"));
            };

            return RedirectToAction("Index");
        }

        public IActionResult Passive(int id = 0)
        {
            MenuProductFeature entity = _featureService.GetWithFeatureItemsById(id);

            if (entity == null)
            {
                return View("Error");
            }
            AppUser User = _userManager.Users.SingleOrDefault(x => x.UserName == HttpContext.User.Identity.Name);
            if (entity.MenuProduct.MenuCategory.RestaurantId != User.RestaurantId)
            {
                return View("Error");
            }

            entity.IsActive = false;
            entity.UpdatedDate = DateTime.Now;
            _featureService.Update(entity);
            _featureItemService.PassivedItems(entity.ProductFeatureItems);
            TempData.Add("messagex", String.Format($"{_locService.GetLocalizedValue("SuccessProcess")}*success"));
            return RedirectToAction("Index");
        }

        public IActionResult Activated(int id = 0)
        {
            MenuProductFeature entity = _featureService.GetWithFeatureItemsById(id);

            if (entity == null)
            {
                return View("Error");
            }
            AppUser User = _userManager.Users.SingleOrDefault(x => x.UserName == HttpContext.User.Identity.Name);
            if (entity.MenuProduct.MenuCategory.RestaurantId != User.RestaurantId)
            {
                return View("Error");
            }

            entity.IsActive = true;
            entity.UpdatedDate = DateTime.Now;
            _featureService.Update(entity);
            _featureItemService.ActivatedItems(entity.ProductFeatureItems);
            TempData.Add("messagex", String.Format($"{_locService.GetLocalizedValue("SuccessProcess")}*success"));
            return RedirectToAction("Index");
        }

        public JsonResult AddFeatureItem(int featureId, string items)
        {
            AppUser User = _userManager.Users.SingleOrDefault(x => x.UserName == HttpContext.User.Identity.Name);
            MenuProductFeature entity = _featureService.GetWithFeatureItemsById(featureId);
            if (entity == null)
            {
                return Json("500");
            }

            if (entity.MenuProduct.MenuCategory.RestaurantId != User.RestaurantId)
            {
                return Json("500");
            }

            try
            {
                MenuProductFeatureItem featureItem = new MenuProductFeatureItem()
                {
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    Name = items.Split("?")[0],
                    Price = !String.IsNullOrEmpty(items.Split("?")[1]) ? Convert.ToDecimal(items.Split("?")[1]) : 0,
                    ProductFeatureId = entity.Id
                };
                _featureItemService.Add(featureItem);
                return Json(featureItem.Id);
            }
            catch (Exception)
            {
                return Json("500");
            }
        }
        public JsonResult DeleteFeatureItem(int featureId)
        {
            AppUser User = _userManager.Users.SingleOrDefault(x => x.UserName == HttpContext.User.Identity.Name);
            MenuProductFeatureItem entity = _featureItemService.GetById(featureId);
            MenuProductFeature feature = _featureService.GetWithFeatureItemsById(entity.ProductFeatureId);
            if (entity == null)
            {
                return Json("500");
            }

            if (feature.MenuProduct.MenuCategory.RestaurantId != User.RestaurantId)
            {
                return Json("500");
            }
            _featureItemService.HardDelete(featureId);
            return Json("200");
        }
    }
}
