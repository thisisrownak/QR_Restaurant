using QR_Restaurant.Data.Entities;
using System.Collections.Generic;

namespace QR_Restaurant.UI
{
    public class RestaurantMenuViewModel
    {
        public IEnumerable<MenuCategory> CategoriesWithProducts { get; internal set; }
        public Restaurant Restaurant { get; internal set; }
        public int TableId { get; internal set; }
    }
}