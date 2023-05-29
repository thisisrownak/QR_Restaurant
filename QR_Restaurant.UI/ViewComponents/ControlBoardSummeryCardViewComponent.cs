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
    public class ControlBoardSummeryCardViewComponent : ViewComponent
    {
        private readonly IPaymentService _paymentService;
        private readonly IOrderService _orderService;
        protected readonly UserManager<AppUser> _userManager;
        public ControlBoardSummeryCardViewComponent(IPaymentService paymentService, UserManager<AppUser> userManager, IOrderService orderService)
        {
            _paymentService = paymentService;
            _userManager = userManager;
            _orderService = orderService;
        }

        public ViewViewComponentResult Invoke()
        {
            var User = _userManager.Users.Where(x => x.UserName == HttpContext.User.Identity.Name)
                  .Include(x => x.Restaurant)
                  .AsNoTracking()
                  .SingleOrDefault();

            var count = _userManager.Users.Where(x => x.RestaurantId == (int)User.RestaurantId).Count();

            ControlBoardSummeryViewModel model = new ControlBoardSummeryViewModel();
            model.TotalSalesAmount = _paymentService.GetSalesAmount((int)User.RestaurantId);
            model.TotalOrderCount = _orderService.GetOrderCount((int)User.RestaurantId);
            model.TotalEmployees = count;
            return View(model);
        }
    }
}
