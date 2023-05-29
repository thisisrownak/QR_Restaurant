using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using QR_Restaurant.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QR_Restaurant.UI.ViewComponents
{
    public class UserSummaryViewComponent : ViewComponent
    {
       
        public ViewViewComponentResult Invoke()
        {
            return View();
        }
    }
}
