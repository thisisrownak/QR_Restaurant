using QR_Restaurant.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR_Restaurant.Business.Abstract
{
    public interface IMenuCategoryService
    {
        void Add(MenuCategory entity);
        void Update(MenuCategory entity);
        void SoftDelete(MenuCategory entity);
        void HardDelete(int id);
        MenuCategory GetMenuCategory(int id);
        IEnumerable<MenuCategory> GetAll();
        IEnumerable<MenuCategory> GetAllByRestaurant(int restaurantId);
        IEnumerable<MenuCategory> GetMenuCategoryWithProductsByRestaurant(int restaurantId);
        IEnumerable<MenuCategory> GetActiveCategory();

    }
}
