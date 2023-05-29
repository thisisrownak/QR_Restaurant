using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QR_Restaurant.Business.Abstract;
using QR_Restaurant.UI.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QR_Restaurant.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        IRestaurantService _restorantService;
        IOrderService _orderService;

        public HomeController(IRestaurantService restorantService, IOrderService orderService)
        {
            _restorantService = restorantService;
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            AdminHomeViewModel model = new AdminHomeViewModel()
            {
                ActiveRestaurantCount = _restorantService.GetActiveRestaurantCount(),
                PassiveRestaurantCount = _restorantService.GetPassiveRestaurantCount(),
                CompletedOrderCount = _orderService.GetCompletedOrderCount(),
                OrderEarnings = _orderService.GetOrderEarnings()
            };
            return View(model);
        }

        public IActionResult Settings()
        {
            return View();
        }
    }
}
