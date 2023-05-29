using QR_Restaurant.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR_Restaurant.Business.Abstract
{
    public interface IOrderService
    {
        void Add(Order entity);
        void Update(Order entity);
        void BulkUpdate(List<Order> entities);
        void SoftDelete(Order entity);
        void HardDelete(long id);
        Order GetById(long id);
        IEnumerable<Order> GetAll();
        IEnumerable<Order> GetActiveOrders(int restaurantId);
        IEnumerable<Order> GetOrders(int restaurantId);
        IEnumerable<Order> GetCompletedOrdersInToday(int restaurantId);
        IEnumerable<Order> GetActiveOrdersByTable(int tableId);
        IEnumerable<Order> GetWaiterOrders(string waiterOrderId, DateTime startDate, DateTime endDate);
        IEnumerable<Order> GetRestaurantOrders(int restaurantId, DateTime startDate, DateTime endDate);
        IEnumerable<Order> GetKanbanOrders(int restaurantId);
        decimal GetOrderEarnings();
        int GetCompletedOrderCount();
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
        int GetOrderCount(int restaurantId);
    }
}
