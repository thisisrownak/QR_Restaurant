using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR_Restaurant.Data.Enums
{
    public enum OrderStatus
    {
        [Description("Message")]
        [Display(Name = "Message")]
        Message,
        [Description("On Request")]
        [Display(Name = "On Request")]
        OnRequest,
        [Description("Approved")]
        [Display(Name = "Approved")]
        Approved,
        [Description("Completed")]
        [Display(Name = "Completed")]
        Completed,
        [Description("Rejected")]
        [Display(Name = "Rejected")]
        Rejected,
        [Description("Delivery")]
        [Display(Name = "Delivery")]
        Delivery
    }
}
