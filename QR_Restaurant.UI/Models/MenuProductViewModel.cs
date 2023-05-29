using Microsoft.AspNetCore.Http;
using QR_Restaurant.Data.Entities;
using System.Collections.Generic;

namespace QR_Restaurant.UI.Models
{
    public class MenuProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public IFormFile Photo { get; set; }
        public string PhotoUrl { get; set; }
        public int RowNumber { get; set; }
        public int MenuCategoryId { get; set; }
        public IEnumerable<MenuProduct> MenuProducts { get; internal set; }
        public IEnumerable<MenuCategory> MenuCategories { get; internal set; }
        public string RestaurantCurrencySymbol { get; internal set; }
    }
}