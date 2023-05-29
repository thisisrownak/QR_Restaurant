using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QR_Restaurant.Business.Abstract;
using QR_Restaurant.Data.Entities;
using QR_Restaurant.UI.Helper;
using QR_Restaurant.UI.Models;
using QR_Restaurant.UI.SingalR;
using QR_Restaurant.UI.UIService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QR_Restaurant.UI.Controllers
{
    public class OrderController : Controller
    {
        private LocService _locService;
        private IOrderService _orderService;
        private ICartSessionService _cartSessionService;
        private UserManager<AppUser> _userManager;
        private IRestaurantService _restaurantService;
        private IQrOrderTableService _tableService;
        private IOrder_MenuProduct_RT_Service _orderMenuProductService;
        private IPaymentService _paymentService;
        private OrderHub _socketService;

        public OrderController(LocService locService, IOrderService orderService, ICartSessionService cartSessionService,
            UserManager<AppUser> userManager, IRestaurantService restaurantService, IQrOrderTableService tableService,
            IOrder_MenuProduct_RT_Service orderMenuProductService, OrderHub socketService, IPaymentService paymentService)
        {
            _locService = locService;
            _orderService = orderService;
            _cartSessionService = cartSessionService;
            _userManager = userManager;
            _restaurantService = restaurantService;
            _tableService = tableService;
            _orderMenuProductService = orderMenuProductService;
            _paymentService = paymentService;
            _socketService = socketService;
        }

        [HttpPost]
        public IActionResult AddOrder(AddOrderModel model)
        {
            var cart = _cartSessionService.GetCart();

            if (cart == null)
            {
                return View("Error");
            }

            if (cart.CartContents.Count == 0 || cart.CartContents.SingleOrDefault(x => !x.MenuProduct.IsActive) != null)
            {
                return View("Error");
            }

            var User = _userManager.Users.Where(x => x.Id == model.UserId)
            .Include(x => x.Restaurant)
                .SingleOrDefault();
            if (User == null)
            {
                return View("Error");
            }
            Restaurant restaurant = _restaurantService.GetRestaurant(model.RestaurantId);
            if (restaurant == null || User.RestaurantId != restaurant.Id)
            {
                return View("Error");
            }
            QrOrderTable qrTable = _tableService.GetQrOrderTable(model.TableId);
            if (qrTable == null || qrTable.RestaurantId != restaurant.Id)
            {
                return View("Error");
            }

            try
            {
                _orderService.BeginTransaction();
                Order order = new Order()
                {
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    Note = String.IsNullOrEmpty(model.OrderNote) ? String.Empty : model.OrderNote,
                    QrOrderTableId = qrTable.Id,
                    Total = cart.Total,
                    OrderStatus = Data.Enums.OrderStatus.OnRequest
                };
                _orderService.Add(order);

                _orderMenuProductService.AddBulk(cart.CartContents.Select(x => new Order_MenuProduct_RT()
                {
                    OrderId = order.Id,
                    MenuProductId = x.MenuProduct.Id,
                    Quantity = x.Quantity,
                    SalePrice = x.MenuProduct.Price,
                    FeatureItemsList = x.ProductFeatures,
                    MenuProductFeatureItemIds = x.ProductFeaturesIds == null ? "" : x.ProductFeaturesIds.Replace(" ", "")
                }));

                qrTable.IsRead = false;
                _tableService.Update(qrTable);

                cart = null;
                _cartSessionService.SetCart(cart);
                _orderService.CommitTransaction();
            }
            catch (Exception e)
            {
                _orderService.RollbackTransaction();
                return View("Error");
            }
            _socketService.SendOrder(restaurant.Id.ToString(), qrTable.Id).Wait();
            TempData.Add("messagex", String.Format($"{_locService.GetLocalizedValue("AddOrderSuccessMessage")}*success"));
            return Redirect($"/restaurant/menu?id={restaurant.Id}&tableNo={qrTable.Id}&userId={User.Id}");
        }

        [Authorize(Roles = "RestaurantAdmin, Admin, Waiter")]
        public JsonResult GetActiveOrders()
        {
            var User = _userManager.Users.Where(x => x.UserName == HttpContext.User.Identity.Name)
           .Include(x => x.Restaurant)
               .SingleOrDefault();

            var Staff = _userManager.Users.Where(x => x.RestaurantId == User.RestaurantId && x.Id != User.Id && x.EmailConfirmed).Select(x => new StaffComplateModel()
            {
                UserId = x.Id,
                Avatar = x.Avatar
            }).ToList();

            var model = _orderService.GetActiveOrders((int)User.RestaurantId).Select(x => new RestaurantOrderJsonModel()
            {
                Id = x.Id,
                TableName = x.QrOrderTable.Name,
                TableNo = x.QrOrderTable.TableNo,
                Status = _locService.GetLocalizedValue(x.OrderStatus.ToString()),
                Note = x.Note,
                Date = x.CreatedDate.ToLongDateString() == DateTime.Now.ToLongDateString() ? x.CreatedDate.ToShortTimeString() : x.CreatedDate.ToLongDateString(),
                Products = x.Order_MenuProducts.Select(x => x.MenuProduct.Name).ToList(),
                ProductFeatures = x.Order_MenuProducts.Select(x => x.FeatureItemsList).ToList(),
                Quantities = x.Order_MenuProducts.Select(x => x.Quantity).ToList(),
                Staff = Staff
            });

            return Json(model);
        }

        [Authorize(Roles = "RestaurantAdmin, Admin, Waiter")]
        public JsonResult GetCompletedOrders()
        {
            var User = _userManager.Users.Where(x => x.UserName == HttpContext.User.Identity.Name)
           .Include(x => x.Restaurant)
               .SingleOrDefault();

            var model = _orderService.GetCompletedOrdersInToday((int)User.RestaurantId).Select(x => new RestaurantOrderJsonModel()
            {
                Id = x.Id,
                TableName = x.QrOrderTable.Name,
                TableNo = x.QrOrderTable.TableNo,
                Status = _locService.GetLocalizedValue(Enumeration.GetEnumDescription(x.OrderStatus)),
                Note = x.Note,
                Date = x.CreatedDate.ToLongDateString() == DateTime.Now.ToLongDateString() ? x.CreatedDate.ToShortTimeString() : x.CreatedDate.ToLongDateString(),
                Products = x.Order_MenuProducts.Select(x => x.MenuProduct.Name).ToList(),
                ProductFeatures = x.Order_MenuProducts.Select(x => x.FeatureItemsList).ToList(),
                Quantities = x.Order_MenuProducts.Select(x => x.Quantity).ToList(),
                Total = x.Total
            }).ToList();

            return Json(model);
        }

        [Authorize(Roles = "RestaurantAdmin, Admin, Waiter")]
        public JsonResult CompleteOrder(long id, string staffId)
        {
            Order Order = _orderService.GetById(id);

            if (Order == null)
            {
                return Json("Order is null.*danger");
            }

            if (Order.OrderStatus == Data.Enums.OrderStatus.OnRequest || Order.OrderStatus == Data.Enums.OrderStatus.Approved)
            {
                Order.UpdatedDate = DateTime.Now;
                Order.DeliveringWaiter = staffId;
                Order.OrderStatus = Data.Enums.OrderStatus.Completed;
                Order.IsActive = false;
                _orderService.Update(Order);

                return Json($"{_locService.GetLocalizedValue("SuccessProcess")}*success");
            }
            return Json("The request could not be fulfilled.*danger");

        }

        [Authorize(Roles = "RestaurantAdmin, Admin, Waiter")]
        public JsonResult CancelOrder(long id)
        {
            Order entity = _orderService.GetById(id);
            if (entity != null && (entity.OrderStatus == Data.Enums.OrderStatus.OnRequest || entity.OrderStatus == Data.Enums.OrderStatus.Approved))
            {
                entity.IsActive = false;
                entity.UpdatedDate = DateTime.Now;
                entity.OrderStatus = Data.Enums.OrderStatus.Rejected;
                _orderService.Update(entity);

                return Json($"{_locService.GetLocalizedValue("SuccessProcess")}*success");
            }
            return Json("The request could not be fulfilled.*danger");
        }

        [Authorize(Roles = "RestaurantAdmin, Admin, Waiter")]
        public JsonResult PayOrders(int tableId)
        {
            var User = _userManager.Users.SingleOrDefault(x => x.UserName == HttpContext.User.Identity.Name);

            QrOrderTable table = _tableService.GetQrOrderTableWithOpenOrders(tableId);

            if (table == null)
            {
                return Json("Not valid Id.*danger");
            }

            if (table.RestaurantId != User.RestaurantId)
            {
                return Json("Not valid Id.*danger");
            }

            try
            {
                _orderService.BeginTransaction();
                Payment payment = new Payment()
                {
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    QrOrderTableId = tableId,
                    Total = table.Orders.Sum(x => x.Total)
                };

                _paymentService.Add(payment);
                table.Orders.ForEach(x => PayOrder(x, payment.Id));
                _orderService.BulkUpdate(table.Orders);
                _orderService.CommitTransaction();
                _socketService.ListenPayOrder(User.RestaurantId.ToString(), tableId).Wait();
                return Json($"200%{payment.Id}%{_locService.GetLocalizedValue("PaymentSuccessMessage")}");
            }
            catch (Exception)
            {
                _orderService.RollbackTransaction();
                return Json("500%0");
            }
        }

        void PayOrder(Order entity, long paymentId)
        {
            entity.UpdatedDate = DateTime.Now;
            entity.OrderStatus = Data.Enums.OrderStatus.Completed;
            entity.IsActive = false;
            entity.PaymentId = paymentId;
        }

        [Authorize(Roles = "RestaurantAdmin, Admin, Waiter")]
        public JsonResult GetKanbanOrders()
        {
            var User = _userManager.Users.Where(x => x.UserName == HttpContext.User.Identity.Name)
           .Include(x => x.Restaurant)
               .SingleOrDefault();

            var model = _orderService.GetKanbanOrders((int)User.RestaurantId).Select(x => new RestaurantOrderJsonModel()
            {
                Id = x.Id,
                TableId = x.QrOrderTable != null ? x.QrOrderTable.Id : 0,
                TableName = x.QrOrderTable != null ? x.QrOrderTable.Name : "",
                TableNo = x.QrOrderTable != null ? x.QrOrderTable.TableNo : 0,
                StatusNo = x.OrderStatus,
                Status = _locService.GetLocalizedValue(Enumeration.GetEnumDescription(x.OrderStatus)),
                Note = x.Note,
                Date = x.CreatedDate.ToLongDateString() == DateTime.Now.ToLongDateString() ? x.CreatedDate.ToShortTimeString() : x.CreatedDate.ToLongDateString(),
                Products = x.Order_MenuProducts.Select(x => x.MenuProduct.Name).ToList(),
                ProductFeatures = x.Order_MenuProducts.Select(x => x.FeatureItemsList).ToList(),
                Quantities = x.Order_MenuProducts.Select(x => x.Quantity).ToList(),
                Total = x.Total,
                OrderDate = x.CreatedDate.ToShortDateString(),
                OrderTime = x.CreatedDate.ToShortTimeString()
            }).ToList();

            return Json(model);
        }

        [Authorize(Roles = "RestaurantAdmin, Admin, Waiter")]
        public JsonResult UpdateKanbanOrder(long id, string status)
        {
            Order Order = _orderService.GetById(id);

            if (Order == null)
            {
                return Json("Order is null.*danger");
            }

            if(Order.OrderStatus == Data.Enums.OrderStatus.Completed && Order.PaymentId != null)
            {
                return Json("The status of the paid order cannot be changed. Refresh the page.*error");
            }

            switch (status)
            {
                case "todo":
                    if (Order.OrderStatus == Data.Enums.OrderStatus.OnRequest)
                    {
                        return Json("100");
                    }

                    Order.UpdatedDate = DateTime.Now;
                    Order.OrderStatus = Data.Enums.OrderStatus.OnRequest;
                    Order.IsActive = true;
                    _orderService.Update(Order);

                    return Json($"{_locService.GetLocalizedValue("SuccessProcess")}*success");
                case "inprogress":
                    if (Order.OrderStatus == Data.Enums.OrderStatus.Approved)
                    {
                        return Json("100");
                    }

                    Order.UpdatedDate = DateTime.Now;
                    Order.OrderStatus = Data.Enums.OrderStatus.Approved;
                    Order.IsActive = true;
                    _orderService.Update(Order);

                    return Json($"{_locService.GetLocalizedValue("SuccessProcess")}*success");
                case "completed":
                    var User = _userManager.Users.SingleOrDefault(x => x.UserName == HttpContext.User.Identity.Name);
                    if (Order.OrderStatus == Data.Enums.OrderStatus.Completed)
                    {
                        return Json("100");
                    }

                    Order.UpdatedDate = DateTime.Now;
                    Order.OrderStatus = Data.Enums.OrderStatus.Completed;
                    Order.IsActive = false;
                    Order.DeliveringWaiter = User.Id;  //Online user completed.
                    _orderService.Update(Order);

                    return Json($"{_locService.GetLocalizedValue("SuccessProcess")}*success");
                default:
                    return Json("500");
            }
        }
    }
}
