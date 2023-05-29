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
    public class QrOrderTableService : IQrOrderTableService
    {
        private IRepository<QrOrderTable> _qrTableDal;
        private readonly QR_Context _context;

        public QrOrderTableService(IRepository<QrOrderTable> qrTableDal, QR_Context context)
        {
            _qrTableDal = qrTableDal;
            _context = context;
        }
        public void Add(QrOrderTable entity)
        {
            _qrTableDal.Add(entity);
        }

        public IEnumerable<QrOrderTable> GetAll()
        {
            return _qrTableDal.GetAll();
        }

        public QrOrderTable GetQrOrderTable(int id)
        {
            return _context.QrOrderTables.Where(x => x.Id == id)
                .Include(x => x.Restaurant)
                .SingleOrDefault();
        }

        public void HardDelete(int id)
        {
            QrOrderTable entity = _qrTableDal.GetById(id); 
            _qrTableDal.Delete(entity);
        }

        public void SoftDelete(QrOrderTable entity)
        {
            entity.IsActive = false;
            _qrTableDal.Update(entity);
        }

        public void Update(QrOrderTable entity)
        {
            _qrTableDal.Update(entity);
        }
        public void BeginTransaction()
        {
            _qrTableDal.BeginTransaction();
        }
        public void CommitTransaction()
        {
            _qrTableDal.CommitTransaction();
        }
        public void RollbackTransaction()
        {
            _qrTableDal.RollbackTransaction();
        }

        public IEnumerable<QrOrderTable> GetQrTablesWithRestaurant(int restaurantId)
        {
            return _context.QrOrderTables.Where(x => x.RestaurantId == restaurantId)
                .Include(x => x.Restaurant);            
        }

        public IEnumerable<QrOrderTable> GetQrTablesWithActiveOrders(int restaurantId)
        {
            return _context.QrOrderTables.Where(x => x.RestaurantId == restaurantId && x.IsActive)
                .Include(x => x.Orders.Where(o => o.PaymentId == null && o.OrderStatus != Data.Enums.OrderStatus.Rejected));
        }

        public QrOrderTable GetQrOrderTableWithOpenOrders(int id)
        {
            return _context.QrOrderTables.Where(x => x.Id == id)
                .Include(x => x.Orders.Where(x => x.PaymentId == null && x.OrderStatus != Data.Enums.OrderStatus.Rejected))
                .SingleOrDefault();
        }

        public QrOrderTable GetQrOrderTableByNoOrNameForRestaurant(int restaurantId, int? tableNo)
        {
            if(tableNo == null)
            {
                return null;
            }
            return _context.QrOrderTables
                .FirstOrDefault(x => x.RestaurantId == restaurantId && x.TableNo == tableNo);
        }
    }
}
