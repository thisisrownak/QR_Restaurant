using System;

namespace QR_Restaurant.UI.Models
{
    public class ControlBoardSummeryViewModel
    {
        public Decimal? TotalSalesAmount { get; set; } = Decimal.Zero;
        public int? TotalOrderCount { get; set; } = 0;
        public int? TotalEmployees { get; set; } = 0;
    }
}
