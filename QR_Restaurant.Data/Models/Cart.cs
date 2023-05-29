using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR_Restaurant.Data.Models
{
    public class Cart
    {
        public Cart()
        {
            CartContents = new List<CartContent>();
        }
        public List<CartContent> CartContents { get; set; }

        public decimal Total
        {
            get { return CartContents.Sum(c => c.ProductTotal); }
        }
    }
}
