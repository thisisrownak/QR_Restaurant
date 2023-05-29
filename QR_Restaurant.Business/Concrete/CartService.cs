using QR_Restaurant.Business.Abstract;
using QR_Restaurant.Data.Entities;
using QR_Restaurant.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR_Restaurant.Business.Concrete
{
    public class CartService : ICartService
    {
        public void AddToCart(Cart cart, MenuProduct menuProduct, int quantity, string productFeatures, decimal productFeaturesTotal, string productFeaturesIds)
        {
            CartContent cartContent = cart.CartContents.FirstOrDefault(c => c.MenuProduct.Id == menuProduct.Id && c.ProductFeatures == productFeatures);
            if (cartContent != null)
            {
                cartContent.Quantity = cartContent.Quantity + quantity;
                cartContent.ProductTotal += (quantity * cartContent.MenuProduct.Price) +(quantity * productFeaturesTotal);
                return;
            }
            cart.CartContents.Add(new CartContent { MenuProduct = menuProduct, Quantity = quantity, ProductTotal = (quantity * menuProduct.Price) + (quantity * productFeaturesTotal), ProductFeatures = productFeatures, ProductFeaturesIds=productFeaturesIds });
        }

        public bool UpdateToCart(Cart cart, MenuProduct menuProduct, int quantity)
        {
            CartContent cartContent = cart.CartContents.FirstOrDefault(c => c.MenuProduct.Id == menuProduct.Id);
            if (cartContent != null && cartContent.Quantity != quantity)
            {
                cartContent.Quantity = quantity;
                cartContent.ProductTotal = cartContent.Quantity * cartContent.MenuProduct.Price;
                return true;
            }
            return false;
        }

        public List<CartContent> GetList(Cart cart)
        {
            return cart.CartContents;
        }

        public void RemoveToCart(Cart cart, int menuProductId, string featureItems)
        {
            cart.CartContents.Remove(cart.CartContents.FirstOrDefault(c => c.MenuProduct.Id == menuProductId && !String.IsNullOrEmpty(featureItems) ? c.ProductFeatures == featureItems : (c.ProductFeatures == null || c.ProductFeatures == "")));
        }
    }
}
