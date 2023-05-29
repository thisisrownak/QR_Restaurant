using QR_Restaurant.Data.Entities;
using QR_Restaurant.UI.Models;
using System.Collections.Generic;

namespace QR_Restaurant.UI
{
    public class RestaurantManagerViewModel
    {
        public int? Id { get; internal set; }
        public List<RestaurantOrderJsonModel> CompletedOrders { get; set; }
        public IEnumerable<QrOrderTable> Tables { get; internal set; }
        public string CurrencySymbol { get; internal set; }
    }
}