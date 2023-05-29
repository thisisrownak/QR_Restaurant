using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QR_Restaurant.UI.Models
{
    public class LoginViewModel
    {
        public string EMail { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RmemberMe { get; set; }
    }
}
