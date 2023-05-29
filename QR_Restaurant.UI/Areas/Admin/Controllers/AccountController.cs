using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QR_Restaurant.Business.Abstract;
using QR_Restaurant.Data.Entities;
using QR_Restaurant.UI.Areas.Admin.Models;
using QR_Restaurant.UI.Areas.Admin.Validations;
using QR_Restaurant.UI.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QR_Restaurant.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AccountController : Controller
    {
        private UserManager<AppUser> _userManager;
        private RoleManager<AppRole> _roleManager;
        private IRestaurantService _restaurantService;
        private LocService _locService;
        private readonly ValidationLocalizer _validationLocalizer;

        public AccountController(UserManager<AppUser> userManager, IRestaurantService restaurantService, RoleManager<AppRole> roleManager, LocService locService,
            ValidationLocalizer validationLocalizer)
        {
            _userManager = userManager;
            _restaurantService = restaurantService;
            _roleManager = roleManager;
            _locService = locService;
            _validationLocalizer = validationLocalizer;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult RestaurantRegister(RestaurantRegisterModel model)
        {
            RegisterRestaurantValidator validator = new RegisterRestaurantValidator(_userManager, _restaurantService, _validationLocalizer);
            ValidationResult results = validator.Validate(model);
            if (!results.IsValid)
            {
                return Json(results.Errors);
            }

            try
            {
                _restaurantService.BeginTransaction();

                Restaurant entity = new Restaurant()
                {
                    Name = model.RestaurantName,
                    Description = model.RestaurantDescription,
                    ManagerUser = $"{model.ManagerName} {model.ManagerSurname} ({model.ManagerUserName})",
                    LogoUrl = "default.png",
                    CreatedDate = DateTime.Now,
                    Phone = model.RestaurantPhone,
                    Address = String.IsNullOrEmpty(model.RestaurantAdress) ? "" : model.RestaurantAdress,
                    IsActive = true,
                    RestaurantUrl = model.RestaurantUrl,
                    IsShowTableName = true,
                    IsShowTableNo = true
                };

                _restaurantService.Add(entity);

                AppUser user = new AppUser
                {
                    UserName = model.ManagerUserName,
                    Name = model.ManagerName,
                    Surname = model.ManagerSurname,
                    Email = model.ManagerEmail,
                    PhoneNumber = model.ManagerPhone,
                    Avatar = "default.png",
                    RegistrationDate = DateTime.Now,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    RestaurantId = entity.Id
                };

                IdentityResult result = _userManager.CreateAsync(user, model.ManagerPassword).Result;

                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync("RestaurantAdmin").Result)
                    {
                        AppRole role = new AppRole
                        {
                            Name = "RestaurantAdmin"
                        };

                        IdentityResult roleResult = _roleManager.CreateAsync(role).Result;

                        if (!roleResult.Succeeded)
                        {
                            ModelState.AddModelError("", "Role didn't add!");
                            return Json(model);
                        }
                    }
                    _userManager.AddToRoleAsync(user, "RestaurantAdmin").Wait();
                }

                _restaurantService.CommitTransaction();                
            }
            catch (Exception e)
            {
                _restaurantService.RollbackTransaction();
                return Json(e.Message);
            }

            return Json($"200?{_locService.GetLocalizedValue("SuccessProcess")}*success");
        }
    }
}
