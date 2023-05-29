using QR_Restaurant.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QR_Restaurant.UI.Models
{
    public class SoldStockModel
    {
        public string ProductName { get; internal set; }
        public int ProductQuantity { get; internal set; }
        public string ProductTotal { get; internal set; }
        public decimal UnitPrice { get; internal set; }
        public string ProductFeatures { get; internal set; }
        public IEnumerable<MenuProductFeatureItem> FeatureItems { get; internal set; }
    }
}
