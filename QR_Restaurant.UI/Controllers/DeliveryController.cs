using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QR_Restaurant.Business.Abstract;
using QR_Restaurant.Data.Entities;
using QR_Restaurant.UI.Models;
using System.Linq;

namespace QR_Restaurant.UI.Controllers
{
    public class DeliveryController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDeliveryAreaService _deliveryAreaService;
        private readonly IRestaurantService _restatrantService;
        protected readonly UserManager<AppUser> _userManager;

        public DeliveryController(ILogger<HomeController> logger, IDeliveryAreaService deliveryAreaService, UserManager<AppUser> userManager, IRestaurantService restatrantService)
        {
            _logger = logger;
            _deliveryAreaService = deliveryAreaService;
            _userManager = userManager;
            _restatrantService = restatrantService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Area()
        {
            var user = _userManager.Users.Where(x => x.UserName == HttpContext.User.Identity.Name)
                  .Include(x => x.Restaurant).FirstOrDefault();
            var data = _deliveryAreaService.GetAll();
            ViewBag.DeliveryCharge = user.Restaurant.DeliveryCharge;
            return View(data);
        }

        [HttpPost]
        public IActionResult SaveArea(string Name,double Range,double DeliveryCharge,double Latitude,double Longitude)
        {

            var entity = new DeliveryArea
            {
                Name=Name,
                Range=Range,
                DeliveryCharge=DeliveryCharge,
                Latitude=Latitude,
                Longitude=Longitude
            };
            _deliveryAreaService.Add(entity);
            return Json(new {Status="Ok"});
        }

        [HttpPost]
        public IActionResult UpdateDeliveryCharge(decimal DeliveryCharge)
        {

            var user = _userManager.Users.Where(x => x.UserName == HttpContext.User.Identity.Name)
                  .Include(x => x.Restaurant).FirstOrDefault();
            _restatrantService.BeginTransaction();
            var entity = user.Restaurant;
           entity.DeliveryCharge = DeliveryCharge;
            _restatrantService.Update(entity);
            _restatrantService.CommitTransaction();
            return Json(new { Status = "Ok" });
        }

        public IActionResult Requests()
        {
            return View();
        }
    }
}
