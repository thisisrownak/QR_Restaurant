using QR_Restaurant.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR_Restaurant.Data.Models
{
    public class CartContent
    {
        public MenuProduct MenuProduct { get; set; }
        public int Quantity { get; set; }
        public decimal ProductTotal { get; set; }
        public string ProductFeatures { get; set; }
        public string ProductFeaturesIds { get; set; }
    }
}
