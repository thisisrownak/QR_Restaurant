using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using QR_Restaurant.Data.Entities;
using QR_Restaurant.UI.Helper;
using QR_Restaurant.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QR_Restaurant.UI.Validations
{
    public class AddStaffValidator : AbstractValidator<RestaurantStaffViewModel>
    {
        private readonly ValidationLocalizer _localizer;
        private UserManager<AppUser> _userManager;
        public AddStaffValidator(ValidationLocalizer localizer, UserManager<AppUser> userManager)
        {
            _localizer = localizer;
            _userManager = userManager;

            RuleFor(x => x.UserName).NotEmpty().WithMessage(_localizer.GetLocalizedValue("Username"))
                     .Must(IsUserNameValid).WithMessage(_localizer.GetLocalizedValue("ValidUsername"))
                 .Must(UniqueName).WithMessage(_localizer.GetLocalizedValue("UniqueUsername"))
                 .MaximumLength(40).WithMessage(_localizer.GetLocalizedValue("UsernameMaxLength"));

            RuleFor(x => x.Email)
           .NotEmpty().WithMessage(_localizer.GetLocalizedValue("Email"))
           .EmailAddress().WithMessage(_localizer.GetLocalizedValue("EmailFormat"))
           .Must(UniqueMail).WithMessage(_localizer.GetLocalizedValue("UniqueEmail"))
           .MaximumLength(200).WithMessage(_localizer.GetLocalizedValue("EmailMaxLength"));

            RuleFor(x => x.Password)
               .NotEmpty().WithMessage(_localizer.GetLocalizedValue("Password"))
                .Equal(x => x.PasswordAgain).WithMessage(_localizer.GetLocalizedValue("PasswordAgain"))
               .MinimumLength(6).WithMessage(_localizer.GetLocalizedValue("PasswordMinChar"))
                .Must(IsPasswordValid).WithMessage(_localizer.GetLocalizedValue("ValidPassword"))
               .MaximumLength(35).WithMessage(_localizer.GetLocalizedValue("PasswordMaxChar"));

            RuleFor(x => x.Name)
             .NotEmpty().WithMessage(_localizer.GetLocalizedValue("Name"))
             .MaximumLength(50).WithMessage(_localizer.GetLocalizedValue("MaxName"));

            RuleFor(x => x.Surname)
               .NotEmpty().WithMessage(_localizer.GetLocalizedValue("Surname"))
               .MaximumLength(70).WithMessage(_localizer.GetLocalizedValue("Surname"));

            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage(_localizer.GetLocalizedValue("Phone"))
                       .MaximumLength(15).WithMessage(_localizer.GetLocalizedValue("MaxPhone"));

            RuleFor(x => x.PasswordAgain).NotEmpty().WithMessage(_localizer.GetLocalizedValue("PasswordAgain"));

            RuleFor(x => x.Photo)
                .Must(ImageControl).WithMessage(_localizer.GetLocalizedValue("ImageControl"));

        }

        private bool ImageControl(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                if (!file.ContentType.Contains("image") || file.Length > 6000000)
                    return false;
            }

            return true;
        }

        private bool UniqueMail(string mail)
        {
            var isUserAlreadyExist = _userManager.Users.SingleOrDefault(x => x.Email == mail);

            if (isUserAlreadyExist != null)
            {
                return false;
            }

            return true;
        }

        private bool UniqueName(string userName)
        {
            if (userName != null)
            {
                var isUserAlreadyExist = _userManager.Users.SingleOrDefault(x => x.UserName.ToLower() == userName.ToLower());

                if (isUserAlreadyExist != null)
                {
                    return false;
                }
            }

            return true;
        }
    

        private bool IsUserNameValid(string userName)
        {
            if (userName != null)
            {
                if (userName.Contains(' '))
                {
                    return false;
                }

                //abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789
                List<Char> ValidChars = new List<char>()
                {
                    'a', 'b', 'c','d','e','f','g','h','i','j', 'k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
                    'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z',
                    '0','1','2','3','4','5','6','7','8','9'
                };

                if (!(userName.All(x => ValidChars.Contains(x))))
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsPasswordValid(string password)
        {
            if (password != null)
            {
                if (password.Contains(' '))
                {
                    return false;
                }
            }

            return true;
        }

    }
}
