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
    public class PaymentService : IPaymentService
    {
        private IRepository<Payment> _paymentService;
        private QR_Context _context;

        public PaymentService(IRepository<Payment> paymentService, QR_Context context)
        {
            _paymentService = paymentService;
            _context = context;
        }
        public void Add(Payment entity)
        {
            _paymentService.Add(entity);
        }

        public IEnumerable<Payment> GetAll()
        {
            return _paymentService.GetAll();
        }

        public Payment GetById(long id)
        {
           return _paymentService.GetById(id);
        }

        public void HardDelete(long id)
        {
            Payment entity = _paymentService.GetById(id);
            _paymentService.Delete(entity);
        }

        public void SoftDelete(Payment entity)
        {
            entity.IsActive = false;
            _paymentService.Update(entity);
        }

        public void Update(Payment entity)
        {
            _paymentService.Update(entity);
        }

        public Decimal GetSalesAmount(int restaurantId)
        {
            var payments= _context.Payments.Include(x => x.QrOrderTable)
                .Where(x => x.QrOrderTable.RestaurantId == restaurantId).AsNoTracking().ToList();

            return payments.Select(t => t.Total).Sum();
        }
    }
}
