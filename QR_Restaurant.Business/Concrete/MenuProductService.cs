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
    public class MenuProductService : IMenuProductService
    {
        private IRepository<MenuProduct> _menuProductDal;
        private readonly QR_Context _context;
        public MenuProductService(IRepository<MenuProduct> menuProductDal, QR_Context context)
        {
            _menuProductDal = menuProductDal;
            _context = context;
        }
        public void Add(MenuProduct entity)
        {
            _menuProductDal.Add(entity);
        }

        public IEnumerable<MenuProduct> GetAll()
        {
            return _menuProductDal.GetAll();
        }

        public IEnumerable<MenuProduct> GetAllByCategory(int categoryId)
        {
            return _menuProductDal.GetAll(x => x.MenuCategoryId == categoryId);
        }

        public MenuProduct GetById(int id)
        {
            return _menuProductDal.GetById(id);
        }

        public MenuProduct GetWithCategoryById(int id)
        {
            return _context.MenuProducts.Where(x => x.Id == id)
                .Include(x => x.MenuCategory)
                .SingleOrDefault();
        }

        public void HardDelete(int id)
        {
            MenuProduct entity = _menuProductDal.GetById(id);
            _menuProductDal.Delete(entity);
        }

        public void SoftDelete(MenuProduct entity)
        {
            entity.IsActive = false;
            _menuProductDal.Update(entity);
        }

        public void Update(MenuProduct entity)
        {
            _menuProductDal.Update(entity);
        }

        public IEnumerable<MenuProduct> GetAllByRestaurant(int restaurantId)
        {
            return _context.MenuProducts
               .Include(x => x.MenuCategory)
               .Where(x => x.MenuCategory.RestaurantId == restaurantId);
        }

        public int GetItemsCount(int restaurantId)
        {
            return _context.MenuProducts.Include(x => x.MenuCategory)
                .Where(x => x.MenuCategory.RestaurantId == restaurantId).AsNoTracking().Count();
        }
    }
}
