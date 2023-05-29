using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;
using QR_Restaurant.Business.Abstract;
using QR_Restaurant.Data.Entities;
using QR_Restaurant.UI.Models;
using System.Linq;

namespace QR_Restaurant.UI.ViewComponents
{
    public class ControlBoardCardsViewComponent : ViewComponent
    {
        protected readonly IOrderService _orderService;
        protected readonly IPaymentService _paymentService;
        protected readonly IMenuProductService _menuProductService;
        protected readonly UserManager<AppUser> _userManager;
        public ControlBoardCardsViewComponent(IOrderService orderService, UserManager<AppUser> userManager, IPaymentService paymentService, IMenuProductService menuProductService)
        {
            _orderService = orderService;
            _userManager = userManager;
            _paymentService = paymentService;
            _menuProductService = menuProductService;
        }
        public ViewViewComponentResult Invoke()
        {
            var User = _userManager.Users.Where(x => x.UserName == HttpContext.User.Identity.Name)
                  .Include(x => x.Restaurant)
                  .AsNoTracking()
                  .SingleOrDefault();

            ControlBoardCountViewModel model = new ControlBoardCountViewModel();
            model.TotalOrderCount = _orderService.GetOrderCount((int)User.RestaurantId);
            model.TotalOrderAmount = _paymentService.GetSalesAmount((int)User.RestaurantId);
            model.ItemsCount = _menuProductService.GetItemsCount((int)User.RestaurantId);
            return View(model);
        }
    }
}
