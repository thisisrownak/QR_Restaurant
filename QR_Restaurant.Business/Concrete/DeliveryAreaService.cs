using QR_Restaurant.Business.Abstract;
using QR_Restaurant.Core.Repository;
using QR_Restaurant.Data.Entities;
using System.Collections.Generic;

namespace QR_Restaurant.Business.Concrete
{
    public class DeliveryAreaService:IDeliveryAreaService
    {
        private readonly IRepository<DeliveryArea> _deliveryAreaDal;
        private readonly IRepository<Restaurant> _restaurantareaDal;

        public DeliveryAreaService(IRepository<DeliveryArea> deliveryAreaDal)
        {
            _deliveryAreaDal = deliveryAreaDal;
        }

        public void Add(DeliveryArea entity)
        {
            _deliveryAreaDal.Add(entity);
        }

        public DeliveryArea GetById(long id)
        {
            return _deliveryAreaDal.GetById(id);
        }

        public void HardDelete(int id)
        {
            DeliveryArea entity = _deliveryAreaDal.GetById(id);
            _deliveryAreaDal.Delete(entity);
        }

        public void SoftDelete(DeliveryArea entity)
        {
            entity.IsActive = false;
            _deliveryAreaDal.Update(entity);
        }

        public void Update(DeliveryArea entity)
        {
            _deliveryAreaDal.Update(entity);
        }

        public void Update(Restaurant entity)
        {
            _restaurantareaDal.Update(entity);
        }

        public IEnumerable<DeliveryArea> GetAll()
        {
            return _deliveryAreaDal.GetAll();
        }

        public void BeginTransaction()
        {
            _deliveryAreaDal.BeginTransaction();
        }
        public void CommitTransaction()
        {
            _deliveryAreaDal.CommitTransaction();
        }
        public void RollbackTransaction()
        {
            _deliveryAreaDal.RollbackTransaction();
        }

    }
}
