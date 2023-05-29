using FluentValidation;
using Microsoft.AspNetCore.Http;
using QR_Restaurant.UI.Helper;
using QR_Restaurant.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QR_Restaurant.UI.Validations
{
    public class RestaurantSettingsValidator : AbstractValidator<RestaurantSettingsViewModel>
    {
        private readonly ValidationLocalizer _localizer;
        public RestaurantSettingsValidator(ValidationLocalizer localizer)
        {
            _localizer = localizer;

            RuleFor(x => x.Name).NotEmpty().WithMessage(_localizer.GetLocalizedValue("NotEmpty"))
                .MaximumLength(500).WithMessage(_localizer.GetLocalizedValue("MaxLength"));

            RuleFor(x => x.Description).NotEmpty().WithMessage(_localizer.GetLocalizedValue("NotEmpty"))
              .MaximumLength(1000).WithMessage(_localizer.GetLocalizedValue("MaxLength"));

            RuleFor(x => x.Phone).NotEmpty().WithMessage(_localizer.GetLocalizedValue("Phone"))
                       .MaximumLength(15).WithMessage(_localizer.GetLocalizedValue("MaxPhone"));

            RuleFor(x => x.Logo)
                .Must(ImageControl).WithMessage(_localizer.GetLocalizedValue("ImageControl"));

            RuleFor(x => x.CurrencySymbol)
            .MaximumLength(4).WithMessage(_localizer.GetLocalizedValue("MaxLength"));

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

    }
}
