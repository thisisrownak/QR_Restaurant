using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QR_Restaurant.UI.Areas.Admin.Models
{
    public class AdminHomeViewModel
    {
        public int PassiveRestaurantCount { get; internal set; }
        public int ActiveRestaurantCount { get; internal set; }
        public int CompletedOrderCount { get; internal set; }
        public decimal OrderEarnings { get; internal set; }
    }
}
