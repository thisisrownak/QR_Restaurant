using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QR_Restaurant.UI.Models
{
    public class RestaurantSettingsViewModel
    {
        public int RestaurantId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LogoUrl { get; set; }
        public IFormFile Logo { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public bool IsShowTableName { get; set; }
        public bool  IsShowTableNo { get; set; }
        public string CurrencySymbol { get; set; }
    }
}
