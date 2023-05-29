using QR_Restaurant.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR_Restaurant.Data.Entities
{
    public class Order
    {
        public long Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public string Note { get; set; }
        public decimal Total { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public string DeliveringWaiter { get; set; }
        public List<Order_MenuProduct_RT> Order_MenuProducts { get; set; }
        public int QrOrderTableId { get; set; }
        public QrOrderTable QrOrderTable { get; set; }
        public long? PaymentId { get; set; }
        public Payment Payment { get; set; }
    }
}
