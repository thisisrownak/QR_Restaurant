using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR_Restaurant.Data.Entities
{
    public class Restaurant : BaseEntity
    {
        public string RestaurantUrl { get; set; }  //extra
        public string Name { get; set; }
        public string  Description { get; set; }
        public string LogoUrl { get; set; }
        public string ManagerUser { get; set; }
        public DateTime LastBlockDate { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public bool IsShowTableName { get; set; }
        public bool IsShowTableNo { get; set; }
        public string CurrencySymbol { get; set; }
        public List<AppUser> Users { get; set; }
        public List<QrOrderTable> QrOrderTables { get; set; }
        public decimal? DeliveryCharge { get; set; }

    }
}
