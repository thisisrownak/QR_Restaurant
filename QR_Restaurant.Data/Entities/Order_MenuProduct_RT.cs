using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR_Restaurant.Data.Entities
{
    public class Order_MenuProduct_RT
    {
        public long OrderId { get; set; }
        public Order Order { get; set; }
        public int MenuProductId { get; set; }
        public MenuProduct MenuProduct { get; set; }
        public int Quantity { get; set; }
        public decimal SalePrice { get; set; }
        //FeatureItemsList: The features selected in the order are written by placing a comma between them.
        public string? FeatureItemsList { get; set; }

        //MenuProductFeatureItemIds : Feature Id list selected in the order.
        public string MenuProductFeatureItemIds { get; set; }

        //FeatureItemNameAndPriceList : Names and prices are separated by a question mark. The general separator is sharp.
        //Sample : Tomato?2#Onion?3
        //public string FeatureItemNameAndPriceList { get; set; }

    }
}
