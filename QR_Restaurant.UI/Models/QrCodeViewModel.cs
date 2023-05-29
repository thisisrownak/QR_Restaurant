using QR_Restaurant.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QR_Restaurant.UI.Models
{
    public class QrCodeViewModel
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; }
        public int TableNo { get; set; }
        public string QrCodeUrl { get; set; }
        public int RestaurantId { get; set; }
        public IEnumerable<QrOrderTable> QrOrderTables { get; set; }
        public string HasError { get; set; }
        public bool IsShowTableNo { get; internal set; }
        public bool IsShowTableName { get; internal set; }
        public int OldTableNo { get; set; }
    }
}
