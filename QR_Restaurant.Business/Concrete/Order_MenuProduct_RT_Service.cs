using Microsoft.EntityFrameworkCore;
using QR_Restaurant.Business.Abstract;
using QR_Restaurant.Core.Repository;
using QR_Restaurant.DAL.Context;
using QR_Restaurant.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace QR_Restaurant.Business.Concrete
{
    public class Order_MenuProduct_RT_Service : IOrder_MenuProduct_RT_Service
    {
        private IRepository<Order_MenuProduct_RT> _orderProductDal;
        private QR_Context _context;

        public Order_MenuProduct_RT_Service(IRepository<Order_MenuProduct_RT> orderProductService, QR_Context context)
        {
            _orderProductDal = orderProductService;
            _context = context;
        }

        public void AddBulk(IEnumerable<Order_MenuProduct_RT> entities)
        {
            _orderProductDal.BulkAdd(entities);
        }

        public void DeleteBulk(IEnumerable<Order_MenuProduct_RT> entities)
        {
            _orderProductDal.BulkDelete(entities);
        }

        //public IQueryable<IGrouping<string, Order_MenuProduct_RT>> GetTablePayment(int qrTableId, long paymentId)
        public IEnumerable<Order_MenuProduct_RT> GetTablePayment(int qrTableId, long paymentId)
        {
            return _context.Order_MenuProduct_RT
                .Include(x => x.Order)
                .Where(x => x.Order.QrOrderTableId == qrTableId & x.Order.PaymentId == paymentId)
                .Include(x => x.MenuProduct);             
            //.GroupBy(x => new { x.MenuProduct.Name, x.FeatureItemsList });
        }

        public IEnumerable<Order_MenuProduct_RT> GetRestaurantSoldStocks(int restaurantId, DateTime startDate, DateTime endDate)
        {
            return _context.Order_MenuProduct_RT
               .Include(x => x.Order)
               .ThenInclude(x => x.QrOrderTable)
               .Where(x => x.Order.QrOrderTable.RestaurantId == restaurantId & !x.Order.IsActive & x.Order.OrderStatus == Data.Enums.OrderStatus.Completed
               & x.Order.UpdatedDate >= startDate & x.Order.UpdatedDate <= endDate)
               .Include(x => x.MenuProduct);
        }
    }
}
