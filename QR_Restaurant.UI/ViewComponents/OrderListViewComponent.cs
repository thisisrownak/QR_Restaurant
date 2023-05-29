using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;
using QR_Restaurant.Business.Abstract;
using QR_Restaurant.Data.Entities;
using QR_Restaurant.UI.Helper;
using QR_Restaurant.UI.Models;
using System;
using System.Linq;

namespace QR_Restaurant.UI.ViewComponents
{
    public class OrderListViewComponent : ViewComponent
    {
        protected readonly UserManager<AppUser> _userManager;
        protected readonly IOrderService _orderService;
        protected readonly LocService _locService;
        public OrderListViewComponent(UserManager<AppUser> userManager, IOrderService orderService, LocService locService)
        {
            _userManager = userManager;
            _orderService = orderService;
            _locService = locService;
        }


        public ViewViewComponentResult Invoke()
        {
            var User = _userManager.Users.Where(x => x.UserName == HttpContext.User.Identity.Name)
           .Include(x => x.Restaurant)
               .SingleOrDefault();

            var model = _orderService.GetOrders((int)User.RestaurantId).Select(x => new RestaurantOrderJsonModel()
            {
                Id = x.Id,
                TableName = x.QrOrderTable.Name,
                TableNo = x.QrOrderTable.TableNo,
                Status = _locService.GetLocalizedValue(Enumeration.GetEnumDescription(x.OrderStatus)),
                Note = x.Note,
                Date = x.CreatedDate.ToLongDateString() == DateTime.Now.ToLongDateString() ? x.CreatedDate.ToLongDateString() : x.CreatedDate.ToLongDateString(),
                Products = x.Order_MenuProducts.Select(x => x.MenuProduct.Name).ToList(),
                ProductFeatures = x.Order_MenuProducts.Select(x => x.FeatureItemsList).ToList(),
                Quantities = x.Order_MenuProducts.Select(x => x.Quantity).ToList(),
                Total = x.Total
            }).ToList();


            return View(model);
        }
    }
}
