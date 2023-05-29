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
    public class MenuProductFeatureService : IMenuProductFeatureService
    {
        private IRepository<MenuProductFeature> _productFeatureDal;
        private readonly QR_Context _context;
        public MenuProductFeatureService(IRepository<MenuProductFeature> productFeatureDal, QR_Context context)
        {
            _productFeatureDal = productFeatureDal;
            _context = context;
        }

        public void BeginTransaction()
        {
            _productFeatureDal.BeginTransaction();
        }
        public void CommitTransaction()
        {
            _productFeatureDal.CommitTransaction();
        }
        public void RollbackTransaction()
        {
            _productFeatureDal.RollbackTransaction();
        }
        public void Add(MenuProductFeature entity)
        {
            _productFeatureDal.Add(entity);
        }

        public IEnumerable<MenuProductFeature> GetAll()
        {
            return _productFeatureDal.GetAll();
        }

        public IEnumerable<MenuProductFeature> GetAllByMenuProduct(int menuProductId)
        {
            return _productFeatureDal.GetAll(x => x.MenuProductId == menuProductId);
        }

        public MenuProductFeature GetById(int id)
        {
            return _productFeatureDal.GetById(id);
        }

        public MenuProductFeature GetWithFeatureItemsById(int id)
        {
            return _context.ProductFeatures.Where(x => x.Id == id)
                .Include(x => x.ProductFeatureItems)
                .Include(x => x.MenuProduct)
                .ThenInclude(y => y.MenuCategory)
                .SingleOrDefault();
        }

        public void HardDelete(int id)
        {
            MenuProductFeature entity = _productFeatureDal.GetById(id);
            _productFeatureDal.Delete(entity);
        }

        public void SoftDelete(MenuProductFeature entity)
        {
            entity.IsActive = false;
            _productFeatureDal.Update(entity);
        }

        public void Update(MenuProductFeature entity)
        {
            _productFeatureDal.Update(entity);
        }

        public IEnumerable<MenuProductFeature> GetFeaturesWithItemsByRestaurant(int restaurantId)
        {
            return _context.ProductFeatures
                .Include(x => x.MenuProduct)
                .ThenInclude(y => y.MenuCategory)
                .Where(x => x.MenuProduct.MenuCategory.RestaurantId == restaurantId)
              .Include(x => x.ProductFeatureItems);
        }
    }
}
