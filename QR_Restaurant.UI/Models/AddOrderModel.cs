using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QR_Restaurant.UI.Models
{
    public class AddOrderModel
    {
        public string OrderNote { get; set; }
        public int TableId { get; set; }
        public int RestaurantId { get; set; }
        public string UserId { get; set; }
    }
}
