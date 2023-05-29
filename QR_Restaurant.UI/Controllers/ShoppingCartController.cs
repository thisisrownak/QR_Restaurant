using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QR_Restaurant.Business.Abstract;
using QR_Restaurant.Data.Entities;
using QR_Restaurant.UI.Helper;
using QR_Restaurant.UI.Models;
using QR_Restaurant.UI.UIService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QR_Restaurant.UI.Controllers
{
    public class ShoppingCartController : Controller
    {
        private ICartSessionService _cartSessionService;
        private ICartService _cartService;
        private IMenuProductService _menuProductService;
        private IMenuProductFeatureItemService _featureItemService;
        private LocService _locService;
        private IRestaurantService _restaurantService;
        public ShoppingCartController(ICartSessionService cartSessionService, ICartService cartService, IMenuProductService menuProductService
            , IMenuProductFeatureItemService featureItemService, LocService locService, IRestaurantService restaurantService)
        {
            _cartSessionService = cartSessionService;
            _cartService = cartService;
            _menuProductService = menuProductService;
            _featureItemService = featureItemService;
            _locService = locService;
            _restaurantService = restaurantService;
        }

        public IActionResult Index(int id)
        {
           Restaurant restaurant = _restaurantService.GetRestaurant(id);
            return View("Index", restaurant != null ? restaurant.CurrencySymbol : "");
        }

        public JsonResult GetCartList()
        {
            var cart = _cartSessionService.GetCart();
            CartListJsonModel cartListJsonModel = new CartListJsonModel
            {
                Cart = cart
            };

            return Json(cartListJsonModel);
        }

        [HttpGet]
        public JsonResult AddToCart(int menuProductId, int quantity=1, string productFeaturesIds = "")
        {
            var menuProduct = _menuProductService.GetById(menuProductId);
            quantity = quantity < 0 ?  1 : quantity;
            quantity = quantity > 99 ? 99 : quantity;

            if (menuProduct == null)
            {
                return Json("404");
            }

            string productFeatures = "";
            decimal productFeaturesTotal = 0;

            if (!String.IsNullOrEmpty(productFeaturesIds))
            {
                var featureItems = _featureItemService.GetFeaturesByIds(productFeaturesIds);
                if (featureItems == null)
                {
                    return Json("404");
                }
                productFeatures = String.Join(",", featureItems.Select(x => x.Name));
                productFeaturesTotal = featureItems.Sum(x => x.Price);
            }

            var cart = _cartSessionService.GetCart();

            _cartService.AddToCart(cart, menuProduct, quantity, productFeatures, productFeaturesTotal, productFeaturesIds);
            _cartSessionService.SetCart(cart);
            return Json($"200%{_locService.GetLocalizedValue("AddToCartMessage")}*success");
        }

        public JsonResult RemoveToCart(int menuProductId, string featureItems)
        {
            var cart = _cartSessionService.GetCart();
            _cartService.RemoveToCart(cart, menuProductId, featureItems);
            _cartSessionService.SetCart(cart);
            return Json($"200%{_locService.GetLocalizedValue("RemoveToCartMessage")}*success");
        }

        public JsonResult UpdateToCart(int menuProductId, int quantity = 1)
        {
            var menuProduct = _menuProductService.GetById(menuProductId);
            quantity = quantity < 0 ? 1 : quantity;
            quantity = quantity > 99 ? 99 : quantity;

            if (menuProduct == null)
            {
                return Json("404");
            }

            var cart = _cartSessionService.GetCart();
            bool isChange = _cartService.UpdateToCart(cart, menuProduct, quantity);
            _cartSessionService.SetCart(cart);
            return Json(isChange ? $"200%{_locService.GetLocalizedValue("AddToCartMessage")}*success" : "201%200");
        }
    }
}
