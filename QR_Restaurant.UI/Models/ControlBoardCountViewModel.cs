using System;
using System.Numerics;

namespace QR_Restaurant.UI.Models
{
    public class ControlBoardCountViewModel
    {
        public Decimal? TotalOrderAmount { get; set; } = Decimal.Zero;
        public Decimal? TotalOrderGrouthAmount { get; set; } = Decimal.Zero;
        public int? TotalOrderCount { get; set; } = 0;
        public Decimal? TotalOrderGrouthCount { get; set; } = Decimal.Zero;
        public int? MenuScanCount { get; set; } = 0;
        public Decimal? MenuScanGrouth { get; set; } = Decimal.Zero;
        public int? ItemsCount { get; set; } = 0;
        public Decimal? ItemSectionGrouth { get; set; } = Decimal.Zero;
        public int? ItemSectionsCount { get; set; } = 0;
    }
}
