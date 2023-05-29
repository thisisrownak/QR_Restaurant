using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR_Restaurant.Data.Entities
{
    public class MenuProductFeatureItem : BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int ProductFeatureId { get; set; }
        public MenuProductFeature ProductFeature { get; set; }
    }
}
