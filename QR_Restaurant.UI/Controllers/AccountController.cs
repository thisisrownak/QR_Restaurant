using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using QR_Restaurant.Data.Entities;
using QR_Restaurant.UI.Helper;
using QR_Restaurant.UI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QR_Restaurant.UI.Controllers
{
    public class AccountController : Controller
    {
        public UserManager<AppUser> _userManager { get; set; }
        private SignInManager<AppUser> _signInManager;
        private LocService _locService;
        private RoleManager<AppRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, LocService locService, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _locService = locService;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }
        public IActionResult Login()
        {
            ViewData["Title"] = _locService.GetLocalizedValue("Login");
            
            if(User.IsInRole("Admin"))
            {
                return Redirect("/admin/home");
            }
            else if (User.IsInRole("RestaurantAdmin") || User.IsInRole("Waiter"))
            {
                return Redirect("/Restaurant/Dashboard");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            var User = _userManager.Users.Where(x => x.UserName == loginViewModel.UserName)
           .Include(x => x.Restaurant)
           .SingleOrDefault();

            if (User == null)
            {
                ModelState.AddModelError("", "There is no such user.");
                return View(loginViewModel);
            }

            if (User.Restaurant != null && !User.Restaurant.IsActive)
            {
                return View("LockAccount");
            }

            if (!User.EmailConfirmed)
            {
                ModelState.AddModelError("", "Your registration request is in the approval process.");
                return View(loginViewModel);
            }

            if (!User.PhoneNumberConfirmed)
            {
                ModelState.AddModelError("", "Your account has been frozen.");
                return View(loginViewModel);
            }

            var result = _signInManager.PasswordSignInAsync(loginViewModel.UserName,
                loginViewModel.Password, loginViewModel.RmemberMe, false).Result;

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "You have entered the username or password incorrectly.");
                return View(loginViewModel);
            }

            if (_userManager.IsInRoleAsync(User, "Admin").Result)
            {
                return Redirect("/Admin/home");
            }
            else
            {
                return RedirectToAction("Dashboard", "Restaurant");
            }

        }

        [Authorize(Roles = "RestaurantAdmin, Admin, Waiter")]
        public IActionResult ChangePassword()
        {
            ViewData["Title"] = _locService.GetLocalizedValue("ChangePassword");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "RestaurantAdmin, Admin, Waiter")]
        public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            AppUser User = _userManager.Users.SingleOrDefault(x => x.UserName == HttpContext.User.Identity.Name);
            IdentityResult updateResult = _userManager.ChangePasswordAsync(User, model.OldPassword, model.NewPassword).Result;
            if (updateResult.Succeeded)
            {               
                TempData.Add("messagex", String.Format($"{_locService.GetLocalizedValue("SuccessProcess")}*success"));
                if (_userManager.IsInRoleAsync(User, "Admin").Result)
                {
                    return Redirect("/Admin/home");
                }
                return Redirect("/Restaurant/RestaurantManager");
            }
            ModelState.AddModelError("", "Your current password is not correct.");
            return View(model);
        }       

        public IActionResult LogOut()
        {
            _signInManager.SignOutAsync().Wait();
            return Redirect("/");
        }       

        public IActionResult Denied()
        {
            return View("Denied");
        }

    }
}
