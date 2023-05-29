using Microsoft.AspNetCore.Http;
using QR_Restaurant.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QR_Restaurant.UI.Models
{
    public class RestaurantStaffViewModel
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Password { get; set; }
        public string PasswordAgain { get; set; }
        public IFormFile Photo { get; set; }
        public string PhotoUrl { get; set; }

        public int RestaurantId { get; set; }

        public IEnumerable<AppUser> Staff { get; set; }
    }
}
