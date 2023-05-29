using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QR_Restaurant.UI.Models
{
    public class OrdersOfTableViewModel
    {
        public IEnumerable<RestaurantOrderJsonModel> Orders { get; internal set; }
        public int TableNo { get; internal set; }
        public int TableId { get; internal set; }
        public string CurrencySymbol { get; internal set; }
    }
}
