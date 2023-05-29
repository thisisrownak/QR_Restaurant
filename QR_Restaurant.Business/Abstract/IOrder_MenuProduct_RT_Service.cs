using QR_Restaurant.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR_Restaurant.Business.Abstract
{
    public interface IOrder_MenuProduct_RT_Service
    {
        void AddBulk(IEnumerable<Order_MenuProduct_RT> entities);
        void DeleteBulk(IEnumerable<Order_MenuProduct_RT> entities);
        //IQueryable<IGrouping<string, Order_MenuProduct_RT>> GetTablePayment(int qrTableId, long paymentId);

        IEnumerable<Order_MenuProduct_RT> GetTablePayment(int qrTableId, long paymentId);
        IEnumerable<Order_MenuProduct_RT> GetRestaurantSoldStocks(int restaurantId, DateTime startDate, DateTime endDate);
    }
}
