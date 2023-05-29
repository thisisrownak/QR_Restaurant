using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using QR_Restaurant.Business.Abstract;
using QR_Restaurant.Data.Entities;
using QR_Restaurant.UI.Models;

namespace QR_Restaurant.UI.ViewComponents
{
    public class ControlBoardReviewCardViewComponent : ViewComponent
    {
        private readonly IOrderService _orderService;
        protected readonly UserManager<AppUser> _userManager;
        public ControlBoardReviewCardViewComponent(IOrderService orderService, UserManager<AppUser> userManager)
        {
            _orderService = orderService;
            _userManager = userManager;
        }

        public ViewViewComponentResult Invoke()
        {
            return View(new ControlBoardReviewViewModel());
        }
    }
}
