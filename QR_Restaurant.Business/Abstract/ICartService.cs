using QR_Restaurant.Data.Entities;
using QR_Restaurant.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR_Restaurant.Business.Abstract
{
    public interface ICartService
    {
        void AddToCart(Cart cart, MenuProduct menuProduct, int quantity, string productFeatures, decimal productFeaturesTotal, string productFeaturesIds);
        bool UpdateToCart(Cart cart, MenuProduct menuProduct, int quantity);
        void RemoveToCart(Cart cart, int menuProductId, string featureItems);
        List<CartContent> GetList(Cart cart);
    }
}
