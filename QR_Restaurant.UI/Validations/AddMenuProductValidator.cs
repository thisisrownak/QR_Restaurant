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
    public class AddMenuProductValidator : AbstractValidator<MenuProductViewModel>
    {
        private readonly ValidationLocalizer _localizer;
        public AddMenuProductValidator(ValidationLocalizer localizer)
        {
            _localizer = localizer;

            RuleFor(x => x.Name).NotEmpty().WithMessage(_localizer.GetLocalizedValue("NotEmpty"))
                .MaximumLength(500).WithMessage(_localizer.GetLocalizedValue("MaxLength"));

            RuleFor(x => x.Description).NotEmpty().WithMessage(_localizer.GetLocalizedValue("NotEmpty"))
              .MaximumLength(1000).WithMessage(_localizer.GetLocalizedValue("MaxLength"));

            RuleFor(x => x.Price).NotEqual(0).WithMessage(_localizer.GetLocalizedValue("NotEmpty"))
                 .GreaterThan(0).WithMessage(_localizer.GetLocalizedValue("MinInt"))
                .LessThan(99000000000).WithMessage(_localizer.GetLocalizedValue("MaxInt"));

            RuleFor(x => x.MenuCategoryId).NotEqual(0).WithMessage(_localizer.GetLocalizedValue("NotEmpty"));

            RuleFor(x => x.Photo)
                .Must(ImageControl).WithMessage(_localizer.GetLocalizedValue("ImageControl"));

            RuleFor(x => x.RowNumber).NotEmpty().WithMessage(_localizer.GetLocalizedValue("NotEmpty"))
                .GreaterThan(0).WithMessage(_localizer.GetLocalizedValue("MinInt"))
                .LessThan(10000).WithMessage(_localizer.GetLocalizedValue("MaxInt"))
                .Must(IsRowNoValid).WithMessage(_localizer.GetLocalizedValue("NumberFormat"));
        }

        private bool IsRowNoValid(int tableNo)
        {
            string tableSt = tableNo.ToString();
            if (tableSt != null)
            {
                if (tableSt.Contains(' '))
                {
                    return false;
                }

                //abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789
                List<Char> ValidChars = new List<char>()
                {
                    '0','1','2','3','4','5','6','7','8','9'
                };

                if (!(tableSt.All(x => ValidChars.Contains(x))))
                {
                    return false;
                }
            }

            return true;
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
