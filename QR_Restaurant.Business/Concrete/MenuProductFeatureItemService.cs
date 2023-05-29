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
    public class MenuProductFeatureItemService : IMenuProductFeatureItemService
    {
        private IRepository<MenuProductFeatureItem> _featureItemDal;
        private readonly QR_Context _context;
        public MenuProductFeatureItemService(IRepository<MenuProductFeatureItem> featureItemDal, QR_Context context)
        {
            _featureItemDal = featureItemDal;
            _context = context;
        }
        public void Add(MenuProductFeatureItem entity)
        {
            _featureItemDal.Add(entity);
        }

        public void BulkAdd(List<MenuProductFeatureItem> entities)
        {
            _featureItemDal.BulkAdd(entities);
        }

        public IEnumerable<MenuProductFeatureItem> GetAll()
        {
            return _featureItemDal.GetAll();
        }

        public MenuProductFeatureItem GetById(int id)
        {
            return _featureItemDal.GetById(id);
        }

        public MenuProductFeatureItem GetWithFeatureById(int id)
        {
            return _context.ProductFeatureItems.Where(x => x.Id == id)
                .Include(x => x.ProductFeature)
                .SingleOrDefault();
        }

        public void HardDelete(int id)
        {
            MenuProductFeatureItem entity = _featureItemDal.GetById(id);
            _featureItemDal.Delete(entity);
        }

        public void SoftDelete(MenuProductFeatureItem entity)
        {
            entity.IsActive = false;
            _featureItemDal.Update(entity);
        }

        public void Update(MenuProductFeatureItem entity)
        {
            _featureItemDal.Update(entity);
        }

        public void PassivedItems(List<MenuProductFeatureItem> entities)
        {
            foreach (var item in entities)
            {
                item.IsActive = false;
            }
            _featureItemDal.BulkUpdate(entities);
        }

        public void ActivatedItems(List<MenuProductFeatureItem> entities)
        {
            foreach (var item in entities)
            {
                item.IsActive = true;
            }
            _featureItemDal.BulkUpdate(entities);
        }

        public IEnumerable<MenuProductFeatureItem> GetFeaturesByIds(string ids)
        {
            ids = ids.Remove(ids.Length - 1, 1);
            List<int> idsArray = ids.Split(",").Select(x => Convert.ToInt32(x)).ToList();
            return _featureItemDal.GetAll(x => idsArray.Contains(x.Id));
        }
    }
}
