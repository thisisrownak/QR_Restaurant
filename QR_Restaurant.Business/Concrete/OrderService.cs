using Microsoft.EntityFrameworkCore;
using QR_Restaurant.Business.Abstract;
using QR_Restaurant.Core.Repository;
using QR_Restaurant.DAL.Context;
using QR_Restaurant.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR_Restaurant.Business.Concrete
{
    public class OrderService : IOrderService
    {
        private IRepository<Order> _orderDal;
        private QR_Context _context;

        public OrderService(IRepository<Order> orderDal, QR_Context context)
        {
            _orderDal = orderDal;
            _context = context;
        }
        public void Add(Order entity)
        {
            _orderDal.Add(entity);
        }

        public IEnumerable<Order> GetAll()
        {
            return _orderDal.GetAll();
        }

        public Order GetById(long id)
        {
            return _orderDal.GetById(id);
        }

        public void HardDelete(long id)
        {
            Order entity = _orderDal.GetById(id);
            _orderDal.Delete(entity);
        }

        public void SoftDelete(Order entity)
        {
            entity.IsActive = false;
            _orderDal.Update(entity);
        }

        public void Update(Order entity)
        {
            _orderDal.Update(entity);
        }

        public void BulkUpdate(List<Order> entities)
        {
            _orderDal.BulkUpdate(entities);
        }

        public IEnumerable<Order> GetActiveOrders(int restaurantId)
        {
            return _context.Orders.Include(x => x.QrOrderTable)
                .Where(x => x.QrOrderTable.RestaurantId == restaurantId && x.IsActive)
                .Include(x => x.Order_MenuProducts)
                .ThenInclude(y => y.MenuProduct)
                .OrderBy(x => x.CreatedDate);
        }

        public IEnumerable<Order> GetCompletedOrdersInToday(int restaurantId)
        {
            return _context.Orders.Include(x => x.QrOrderTable)
                .Where(x => x.QrOrderTable.RestaurantId == restaurantId && !x.IsActive && x.CreatedDate.Date == DateTime.Now.Date)
                .Include(x => x.Order_MenuProducts)
                .ThenInclude(y => y.MenuProduct)
                .OrderByDescending(x => x.CreatedDate);
        }

        public IEnumerable<Order> GetOrders(int restaurantId)
        {
            return _context.Orders.Include(x => x.QrOrderTable)
                .Where(x => x.QrOrderTable.RestaurantId == restaurantId)
                .Include(x => x.Order_MenuProducts)
                .ThenInclude(y => y.MenuProduct)
                .OrderByDescending(x => x.CreatedDate);
        }

        public int GetOrderCount(int restaurantId)
        {
            return _context.Orders.Include(x => x.QrOrderTable)
                .Where(x => x.QrOrderTable.RestaurantId == restaurantId).AsNoTracking().Count();
        }

        public IEnumerable<Order> GetActiveOrdersByTable(int tableId)
        {
            return _context.Orders.Include(x => x.QrOrderTable)
                .Where(x => x.QrOrderTable.Id == tableId && x.PaymentId == null && x.OrderStatus != Data.Enums.OrderStatus.Rejected)
                .Include(x => x.Order_MenuProducts)
                .ThenInclude(y => y.MenuProduct)
                .OrderBy(x => x.CreatedDate);
        }

        public IEnumerable<Order> GetWaiterOrders(string waiterOrderId, DateTime startDate, DateTime endDate)
        {
            return _context.Orders.Where(x => x.DeliveringWaiter == waiterOrderId && !x.IsActive && x.UpdatedDate.Date >= startDate.Date && x.UpdatedDate.Date <= endDate)
                .Include(x => x.QrOrderTable)
                .Include(x => x.Order_MenuProducts)
                .ThenInclude(y => y.MenuProduct)
                .OrderByDescending(x => x.CreatedDate);
        }

        public IEnumerable<Order> GetRestaurantOrders(int restaurantId, DateTime startDate, DateTime endDate)
        {
            return _context.Orders.Where(x => !x.IsActive && x.UpdatedDate.Date >= startDate.Date && x.UpdatedDate.Date <= endDate)
                .Include(x => x.QrOrderTable)
                .Where(x => x.QrOrderTable.RestaurantId == restaurantId)
                .Include(x => x.Order_MenuProducts)
                .ThenInclude(y => y.MenuProduct)
                .OrderByDescending(x => x.CreatedDate);
        }

        public IEnumerable<Order> GetKanbanOrders(int restaurantId)
        {
            var svm = _context.Orders.Include(x => x.QrOrderTable)
                .Where(x => x.QrOrderTable.RestaurantId == restaurantId && x.PaymentId == null
                && x.OrderStatus != Data.Enums.OrderStatus.Rejected
                && (x.OrderStatus == Data.Enums.OrderStatus.Completed ? x.CreatedDate >= DateTime.Now.AddMinutes(-300) : true))
                .Include(x => x.Order_MenuProducts)
                .ThenInclude(y => y.MenuProduct)
                .OrderByDescending(x => x.CreatedDate);
            return svm;
        }

        public int GetCompletedOrderCount()
        {
            return _orderDal.GetAll(x => x.PaymentId != null).Count();
        }
        public decimal GetOrderEarnings()
        {
            return _orderDal.GetAll(x => x.PaymentId != null).Sum(x => x.Total);
        }

        public void BeginTransaction()
        {
            _orderDal.BeginTransaction();
        }
        public void CommitTransaction()
        {
            _orderDal.CommitTransaction();
        }
        public void RollbackTransaction()
        {
            _orderDal.RollbackTransaction();
        }
    }
}
