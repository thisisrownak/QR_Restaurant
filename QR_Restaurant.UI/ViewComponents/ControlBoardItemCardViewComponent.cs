using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using QR_Restaurant.Business.Abstract;
using QR_Restaurant.Data.Entities;
using QR_Restaurant.UI.Models;
using System.Linq;

namespace QR_Restaurant.UI.ViewComponents
{
    public class ControlBoardItemCardViewComponent : ViewComponent
    {
        private readonly IOrderService _orderService;
        protected readonly UserManager<AppUser> _userManager;
        public ControlBoardItemCardViewComponent(IOrderService orderService, UserManager<AppUser> userManager)
        {
            _orderService = orderService;
            _userManager = userManager;
        }

        public ViewViewComponentResult Invoke()
        {
            return View(new ControlBoardItemViewModel());
        }
    }
}
