using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR_Restaurant.Data.Entities
{
    public class MenuCategory : BaseEntity
    {
        public string Name { get; set; }
        public int RowNumber { get; set; }
        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
        public List<MenuProduct> MenuProducts { get; set; }
    }
}
