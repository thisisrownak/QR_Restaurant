using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR_Restaurant.Data.Entities
{
    public class QrOrderTable : BaseEntity
    {
        public string Name { get; set; }
        public int TableNo { get; set; }
        public string QrCodeUrl { get; set; }
        public bool IsRead { get; set; }
        public int RestaurantId { get; set; }    
        public Restaurant Restaurant { get; set; }
        public List<Order> Orders { get; set; }
        public List<Payment> Payments { get; set; }

    }
}

