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
    public class MenuCategoryService : IMenuCategoryService
    {
        private IRepository<MenuCategory> _menuCategoryDal;
        private readonly QR_Context _context;

        public MenuCategoryService(IRepository<MenuCategory> menuCategoryDal, QR_Context context)
        {
            _menuCategoryDal = menuCategoryDal;
            _context = context;
        }
        public void Add(MenuCategory entity)
        {
            _menuCategoryDal.Add(entity);
        }

        public MenuCategory GetMenuCategory(int id)
        {
            return _menuCategoryDal.GetById(id);
        }

        public void HardDelete(int id)
        {
            MenuCategory entity = _menuCategoryDal.GetById(id);
            _menuCategoryDal.Delete(entity);
        }

        public void SoftDelete(MenuCategory entity)
        {
            entity.IsActive = false;
            _menuCategoryDal.Update(entity);
        }

        public void Update(MenuCategory entity)
        {
            _menuCategoryDal.Update(entity);
        }

        public IEnumerable<MenuCategory> GetAll()
        {
            return _menuCategoryDal.GetAll();
        }

        public IEnumerable<MenuCategory> GetActiveCategory()
        {
            return _menuCategoryDal.GetAll(x=>x.IsActive==true);
        }

        public IEnumerable<MenuCategory> GetAllByRestaurant(int restaurantId)
        {
            return _menuCategoryDal.GetAll(x => x.RestaurantId == restaurantId);
        }

        public IEnumerable<MenuCategory> GetMenuCategoryWithProductsByRestaurant(int restaurantId)
        {
            return _context.MenuCategories.Where(x => x.RestaurantId == restaurantId)
                .Include(x => x.MenuProducts)
                 .ThenInclude(y => y.MenuProductFeatures.Where(a => a.IsActive))
                .ThenInclude(z => z.ProductFeatureItems.Where(a => a.IsActive));
        }
    }
}
