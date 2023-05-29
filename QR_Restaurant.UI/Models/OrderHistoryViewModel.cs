using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QR_Restaurant.UI.Models
{
    public class OrderHistoryViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<RestaurantOrderJsonModel> Orders { get; internal set; }
        public string CurrencySymbol { get; internal set; }
    }
}
