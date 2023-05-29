using QR_Restaurant.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR_Restaurant.Business.Abstract
{
    public interface IMenuProductService
    {
        void Add(MenuProduct entity);
        void Update(MenuProduct entity);
        void SoftDelete(MenuProduct entity);
        void HardDelete(int id);
        MenuProduct GetById(int id);
        MenuProduct GetWithCategoryById(int id);
        IEnumerable<MenuProduct> GetAll();
        IEnumerable<MenuProduct> GetAllByCategory(int categoryId);
        IEnumerable<MenuProduct> GetAllByRestaurant(int restaurantId);
        int GetItemsCount(int restaurantId);
    }
}
