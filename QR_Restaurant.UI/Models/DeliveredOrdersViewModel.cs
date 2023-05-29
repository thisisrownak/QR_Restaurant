using QR_Restaurant.UI.Models;
using System;
using System.Collections.Generic;

namespace QR_Restaurant.UI
{
    public class DeliveredOrdersViewModel
    {
        public string WaiterId { get; set; }
        public string WaiterNameAndSurname { get; internal set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<RestaurantOrderJsonModel> Orders { get; set; }
        public string CurrencySymbol { get; internal set; }
    }
}