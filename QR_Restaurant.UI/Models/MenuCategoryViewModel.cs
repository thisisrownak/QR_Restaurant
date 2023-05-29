using QR_Restaurant.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QR_Restaurant.UI.Models
{
    public class MenuCategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RowNumber { get; set; }
        public int RestaurantId { get; set; }
        public IEnumerable<MenuCategory> MenuCategories { get; internal set; }
    }
}
