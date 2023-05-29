using QR_Restaurant.Data.Entities;
using System.Collections.Generic;

namespace QR_Restaurant.UI.Models
{
    public class ProductFeatureViewModel
    {
        public IEnumerable<MenuProductFeature> ProductFeatures { get; internal set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string FeatureItemList { get; set; }
        public List<MenuProductFeatureItem> FeatureItems { get; set; }
        public IEnumerable<MenuProduct> MenuProducts { get; internal set; }
        public bool IsMultiSelect { get; set; }
        public int MenuProductId { get; set; }
        public string RestaurantCurrencySymbol { get; internal set; }
    }
}
