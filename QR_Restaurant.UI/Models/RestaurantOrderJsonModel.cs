using QR_Restaurant.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QR_Restaurant.UI.Models
{
    public class RestaurantOrderJsonModel
    {
        public long Id { get; internal set; }
        public string TableName { get; internal set; }
        public int TableNo { get; internal set; }
        public string Status { get; internal set; }
        public string Note { get; internal set; }
        public string Date { get; internal set; }
        public List<string> Products { get; internal set; }
        public List<int> Quantities { get; internal set; }
        public List<StaffComplateModel> Staff { get; internal set; }
        public decimal Total { get; internal set; }
        public OrderStatus StatusNo { get; internal set; }
        public string OrderDate { get; internal set; }
        public string OrderTime { get; internal set; }
        public List<string> ProductFeatures { get; internal set; }
        public int TableId { get; internal set; }
    }
}
