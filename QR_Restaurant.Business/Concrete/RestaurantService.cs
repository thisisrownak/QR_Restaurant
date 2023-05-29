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
    public class RestaurantService : IRestaurantService
    {
        private readonly IRepository<Restaurant> _restaurantDal;

        public RestaurantService(IRepository<Restaurant> restaurantDal)
        {
            _restaurantDal = restaurantDal;
        }
        public void Add(Restaurant entity)
        {
            _restaurantDal.Add(entity);
        }

        public Restaurant GetRestaurant(int id)
        {
            return _restaurantDal.GetById(id);
        }

        public void HardDelete(int id)
        {
            Restaurant entity = _restaurantDal.GetById(id);
            _restaurantDal.Delete(entity);
        }

        public void SoftDelete(Restaurant entity)
        {
            entity.IsActive = false;
            _restaurantDal.Update(entity);
        }

        public void Update(Restaurant entity)
        {
            _restaurantDal.Update(entity);
        }

        public IEnumerable<Restaurant> GetAll()
        {
            return _restaurantDal.GetAll();
        }

        public void BeginTransaction()
        {
            _restaurantDal.BeginTransaction();
        }
        public void CommitTransaction()
        {
            _restaurantDal.CommitTransaction();
        }
        public void RollbackTransaction()
        {
            _restaurantDal.RollbackTransaction();
        }

        public bool IsUniqueUrl(string url)
        {
             Restaurant entity = _restaurantDal.GetAll(x => x.RestaurantUrl == url).SingleOrDefault();

            if (entity == null)
            {
                return true;
            }
            return false;
        }

        public int GetActiveRestaurantCount()
        {
            return _restaurantDal.GetAll(x => x.IsActive).Count();
        }

        public int GetPassiveRestaurantCount()
        {
            return _restaurantDal.GetAll(x => !x.IsActive).Count();
        }


    }
}
