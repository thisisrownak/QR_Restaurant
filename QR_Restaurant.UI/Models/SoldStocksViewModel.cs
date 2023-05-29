using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QR_Restaurant.UI.Models
{
    public class SoldStocksViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IEnumerable<SoldStockModel> SoldStocks { get; internal set; }
        public string RestaurantName { get; internal set; }
        public string RestaurantAddress { get; internal set; }
        public string REstaurantPhone { get; internal set; }
        public string CurrencySymbol { get; internal set; }
    }
}
