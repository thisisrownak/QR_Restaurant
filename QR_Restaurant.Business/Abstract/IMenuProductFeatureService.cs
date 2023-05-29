using QR_Restaurant.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR_Restaurant.Business.Abstract
{
    public interface IMenuProductFeatureService
    {
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
        void Add(MenuProductFeature entity);
        void Update(MenuProductFeature entity);
        void SoftDelete(MenuProductFeature entity);
        void HardDelete(int id);
        MenuProductFeature GetById(int id);
        MenuProductFeature GetWithFeatureItemsById(int id);
        IEnumerable<MenuProductFeature> GetAll();
        IEnumerable<MenuProductFeature> GetAllByMenuProduct(int menuProductId);
        IEnumerable<MenuProductFeature> GetFeaturesWithItemsByRestaurant(int restaurantId);
    }
}
