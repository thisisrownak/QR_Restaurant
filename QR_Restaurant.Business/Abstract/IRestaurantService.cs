using QR_Restaurant.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR_Restaurant.Business.Abstract
{
    public interface IRestaurantService
    {
        void Add(Restaurant entity);
        void Update(Restaurant entity);
        void SoftDelete(Restaurant entity);
        void HardDelete(int id);
        Restaurant GetRestaurant(int id);
        IEnumerable<Restaurant> GetAll();
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();

        bool IsUniqueUrl(string url);
        int GetActiveRestaurantCount();
        int GetPassiveRestaurantCount();

    }
}
