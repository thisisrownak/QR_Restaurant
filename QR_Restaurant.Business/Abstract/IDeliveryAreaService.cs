using QR_Restaurant.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR_Restaurant.Business.Abstract
{
    public interface IDeliveryAreaService
    {
        void Add(DeliveryArea entity);
        void Update(DeliveryArea entity);
        void Update(Restaurant entity);
        void SoftDelete(DeliveryArea entity);
        DeliveryArea GetById(long id);
        IEnumerable<DeliveryArea> GetAll();
    }
}
