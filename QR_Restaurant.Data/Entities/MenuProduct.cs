using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR_Restaurant.Data.Entities
{
    public class MenuProduct : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PhotoUrl { get; set; }
        public int RowNumber { get; set; }
        public int MenuCategoryId { get; set; }
        public MenuCategory MenuCategory { get; set; }
        public List<Order_MenuProduct_RT> Orders_MenuProduct { get; set; }
        public List<MenuProductFeature> MenuProductFeatures { get; set; }
    }
}
