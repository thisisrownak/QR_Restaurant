using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR_Restaurant.Data.Entities
{
    public class Payment
    {
        public long Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public decimal Total { get; set; }
        public int QrOrderTableId { get; set; }
        public QrOrderTable QrOrderTable { get; set; }
        public List<Order> Orders { get; set; }

    }
}
