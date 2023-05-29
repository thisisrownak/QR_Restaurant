using QR_Restaurant.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR_Restaurant.Business.Abstract
{
    public interface IPaymentService
    {
        void Add(Payment entity);
        void Update(Payment entity);
        void SoftDelete(Payment entity);
        void HardDelete(long id);
        Payment GetById(long id);
        IEnumerable<Payment> GetAll();
        Decimal GetSalesAmount(int restaurantId);
    }
}
