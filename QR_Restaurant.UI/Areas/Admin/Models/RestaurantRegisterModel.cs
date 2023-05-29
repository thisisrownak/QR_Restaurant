using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QR_Restaurant.UI.Areas.Admin.Models
{
    public class RestaurantRegisterModel
    {
        public string ManagerUserName { get; set; }
        public string ManagerName { get; set; }
        public string ManagerSurname { get; set; }
        public string ManagerEmail { get; set; }
        public string ManagerPhone { get; set; }
        public string ManagerPassword { get; set; }
        public string PasswordAgain { get; set; }
        public string RestaurantUrl { get; set; }
        public string RestaurantName { get; set; }
        public string RestaurantDescription { get; set; }
        public string RestaurantPhone { get; set; }
        public string RestaurantAdress { get; set; }

    }
}
