using FluentValidation;
using Microsoft.Extensions.Localization;
using QR_Restaurant.UI.Helper;
using QR_Restaurant.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QR_Restaurant.UI.Validations
{
    public class LoginValidator : AbstractValidator<LoginViewModel>
    {
        private readonly ValidationLocalizer _localizer;

        public LoginValidator(ValidationLocalizer localizer)
        {
            _localizer = localizer;

            RuleFor(x => x.UserName).NotEmpty().WithMessage(_localizer.GetLocalizedValue("Username"));

            RuleFor(x => x.Password).NotEmpty().WithMessage(_localizer.GetLocalizedValue("Password"));
        }
    }
}
