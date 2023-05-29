using Microsoft.AspNetCore.Http;
using QR_Restaurant.Data.Models;
using QR_Restaurant.UI.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QR_Restaurant.UI.UIService
{
    public class CartSessionService : ICartSessionService
    {
        private IHttpContextAccessor _httpContextAccessor;

        public CartSessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Cart GetCart()
        {
            Cart sepetCheck = _httpContextAccessor.HttpContext.Session.GetObject<Cart>("cart");
            if (sepetCheck == null)
            {
                _httpContextAccessor.HttpContext.Session.SetObject("cart", new Cart());
                sepetCheck = _httpContextAccessor.HttpContext.Session.GetObject<Cart>("cart");
            }

            return sepetCheck;
        }

        public void SetCart(Cart cart)
        {
            _httpContextAccessor.HttpContext.Session.SetObject("cart", cart);
        }
    }
}
