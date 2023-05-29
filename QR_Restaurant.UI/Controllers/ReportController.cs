using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QR_Restaurant.Business.Abstract;
using QR_Restaurant.Data.Entities;
using QR_Restaurant.UI.Helper;
using QR_Restaurant.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QR_Restaurant.UI.Controllers
{
    public class ReportController : Controller
    {
        private IOrderService _orderService;
        private IOrder_MenuProduct_RT_Service _orderMenuProductService;
        private UserManager<AppUser> _userManager;
        private IQrOrderTableService _tableService;
        private IMenuProductFeatureItemService _featureItemService;
        private LocService _locService;

        public ReportController(IOrderService orderService, UserManager<AppUser> userManager, IOrder_MenuProduct_RT_Service orderMenuProductService
            , LocService locService, IQrOrderTableService tableService, IMenuProductFeatureItemService featureItemService)
        {
            _orderService = orderService;
            _userManager = userManager;
            _orderMenuProductService = orderMenuProductService;
            _tableService = tableService;
            _locService = locService;
            _featureItemService = featureItemService;
        }

        [Authorize(Roles = "RestaurantAdmin, Admin")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "RestaurantAdmin, Admin")]
        public IActionResult OrderHistory()
        {
            OrderHistoryViewModel model = new OrderHistoryViewModel()
            {
                StartDate = DateTime.Now.AddDays(-1),
                EndDate = DateTime.Now
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "RestaurantAdmin, Admin")]
        public IActionResult OrderHistory(OrderHistoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var User = _userManager.Users.Where(x => x.UserName == HttpContext.User.Identity.Name)
             .Include(x => x.Restaurant)
             .SingleOrDefault();

            OrderHistoryViewModel viewModel = new OrderHistoryViewModel()
            {
                Orders = _orderService.GetRestaurantOrders((int)User.RestaurantId, model.StartDate, model.EndDate).Select(x => new RestaurantOrderJsonModel()
                {
                    Id = x.Id,
                    TableName = x.QrOrderTable.Name,
                    TableNo = x.QrOrderTable.TableNo,
                    Status = _locService.GetLocalizedValue(Enumeration.GetEnumDescription(x.OrderStatus)),
                    Note = x.Note,
                    Date = x.CreatedDate.ToLongDateString() == DateTime.Now.ToLongDateString() ? x.CreatedDate.ToShortTimeString() : x.CreatedDate.ToLongDateString(),
                    Products = x.Order_MenuProducts.Select(x => x.MenuProduct.Name).ToList(),
                    Quantities = x.Order_MenuProducts.Select(x => x.Quantity).ToList(),
                    ProductFeatures = x.Order_MenuProducts.Select(x => x.FeatureItemsList).ToList(),
                    Total = x.Total
                }).ToList(),
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                CurrencySymbol = User.Restaurant.CurrencySymbol
            };

            return View(viewModel);
        }

        [Authorize(Roles = "RestaurantAdmin, Admin")]
        public IActionResult SoldStocks()
        {
            SoldStocksViewModel model = new SoldStocksViewModel()
            {
                StartDate = DateTime.Now.AddDays(-1),
                EndDate = DateTime.Now
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "RestaurantAdmin, Admin")]
        public IActionResult SoldStocks(SoldStocksViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var User = _userManager.Users.Where(x => x.UserName == HttpContext.User.Identity.Name)
              .Include(x => x.Restaurant)
              .SingleOrDefault();

            IEnumerable<SoldStockModel> soldStocks = _orderMenuProductService.GetRestaurantSoldStocks((int)User.RestaurantId, model.StartDate, model.EndDate)
                 .GroupBy(x => new { x.MenuProduct.Name, x.FeatureItemsList, x.MenuProductFeatureItemIds })
                .Select(x => new SoldStockModel()
                {
                    ProductName = x.Key.Name,
                    ProductQuantity = x.Sum(t => t.Quantity),
                    //UnitPrice = x.Key.Price,
                    ProductTotal = String.Format("{0:0.00}", x.Sum(t => (t.Quantity * t.SalePrice))),
                    FeatureItems = (x.Key.MenuProductFeatureItemIds != null && x.Key.MenuProductFeatureItemIds != "") ? _featureItemService.GetFeaturesByIds(x.Key.MenuProductFeatureItemIds) : null
                });

            SoldStocksViewModel viewModel = new SoldStocksViewModel()
            {
                SoldStocks = soldStocks,
                RestaurantName = User.Restaurant.Name,
                RestaurantAddress = User.Restaurant.Address,
                REstaurantPhone = User.Restaurant.Phone,
                CurrencySymbol = User.Restaurant.CurrencySymbol,
                StartDate = model.StartDate,
                EndDate =  model.EndDate
            };

            return View("StockResult", viewModel);
        }

        [Authorize(Roles = "RestaurantAdmin, Admin")]
        public IActionResult StockResult()
        {

            return View();
        }

        [Authorize(Roles = "RestaurantAdmin, Admin, Waiter")]
        public IActionResult TablePayment(int tableId, long paymentId)
        {
            var User = _userManager.Users.Where(x => x.UserName == HttpContext.User.Identity.Name)
              .Include(x => x.Restaurant)
              .SingleOrDefault();

            var table = _tableService.GetQrOrderTable(tableId);

            if (table == null || User.RestaurantId != table.RestaurantId)
            {
                return View("Error");
            }

            IEnumerable<SoldStockModel> soldStocks = _orderMenuProductService.GetTablePayment(tableId, paymentId)
                .GroupBy(x => new { x.MenuProduct.Name, x.FeatureItemsList, x.MenuProductFeatureItemIds })
                .Select(x => new SoldStockModel()
                {
                    //(quantity * menuProduct.Price) + (quantity * productFeaturesTotal)
                    ProductName = x.Key.Name,
                    ProductFeatures = x.Key.FeatureItemsList,
                    ProductQuantity = x.Sum(t => t.Quantity),
                    ProductTotal = String.Format("{0:0.00}", x.Sum(t => (t.Quantity * t.SalePrice))),
                    FeatureItems = (x.Key.MenuProductFeatureItemIds != null && x.Key.MenuProductFeatureItemIds != "") ? _featureItemService.GetFeaturesByIds(x.Key.MenuProductFeatureItemIds) : null
                });

            TablePaymentViewModel viewModel = new TablePaymentViewModel()
            {
                SoldStocks = soldStocks,
                RestaurantName = User.Restaurant.Name,
                RestaurantAddress = User.Restaurant.Address,
                REstaurantPhone = User.Restaurant.Phone,
                TableNo = table.TableNo,
                CurrencySymbol = User.Restaurant.CurrencySymbol
            };
            return View(viewModel);
        }
    }
}
