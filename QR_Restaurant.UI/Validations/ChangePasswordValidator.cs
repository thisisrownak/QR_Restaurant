using FluentValidation;
using QR_Restaurant.UI.Helper;
using QR_Restaurant.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QR_Restaurant.UI.Validations
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordViewModel>
    {
        private readonly ValidationLocalizer _localizer;
        public ChangePasswordValidator(ValidationLocalizer localizer)
        {
            _localizer = localizer;

            RuleFor(x => x.OldPassword).NotEmpty().WithMessage(_localizer.GetLocalizedValue("Password"))
                .Equal(x => x.OldPasswordAgain).WithMessage(_localizer.GetLocalizedValue("PasswordAgain"));

            RuleFor(x => x.NewPassword)
               .NotEmpty().WithMessage(_localizer.GetLocalizedValue("NewPassword"))
               .MinimumLength(6).WithMessage(_localizer.GetLocalizedValue("PasswordMinChar"))
                .Must(IsPasswordValid).WithMessage(_localizer.GetLocalizedValue("ValidPassword"))
               .MaximumLength(35).WithMessage(_localizer.GetLocalizedValue("PasswordMaxChar"))
               .Equal(x => x.NewPasswordAgain).WithMessage(_localizer.GetLocalizedValue("PasswordAgain"));

        }

        private bool IsPasswordValid(string userName)
        {
            if (userName != null)
            {
                if (userName.Contains(' '))
                {
                    return false;
                }
            }

            return true;
        }

    }
}
