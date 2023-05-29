using QR_Restaurant.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR_Restaurant.Business.Abstract
{
    public interface IMenuProductFeatureItemService
    {
        void Add(MenuProductFeatureItem entity);
        void BulkAdd(List<MenuProductFeatureItem> entities);
        void Update(MenuProductFeatureItem entity);
        void PassivedItems(List<MenuProductFeatureItem> entities);
        void ActivatedItems(List<MenuProductFeatureItem> entities);
        void SoftDelete(MenuProductFeatureItem entity);
        void HardDelete(int id);
        MenuProductFeatureItem GetById(int id);
        MenuProductFeatureItem GetWithFeatureById(int id);
        IEnumerable<MenuProductFeatureItem> GetAll();
        IEnumerable<MenuProductFeatureItem> GetFeaturesByIds(string ids);
    }
}
