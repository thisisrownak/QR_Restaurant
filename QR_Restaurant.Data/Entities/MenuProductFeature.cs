using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR_Restaurant.Data.Entities
{
    public class MenuProductFeature : BaseEntity
    {
        public string Name { get; set; }
        public bool IsMultiSelect { get; set; }
        public int MenuProductId { get; set; }
        public MenuProduct MenuProduct { get; set; }
        public List<MenuProductFeatureItem> ProductFeatureItems { get; set; }
    }
}
