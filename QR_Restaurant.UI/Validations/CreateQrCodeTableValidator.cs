using FluentValidation;
using QR_Restaurant.Business.Abstract;
using QR_Restaurant.UI.Helper;
using QR_Restaurant.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace QR_Restaurant.UI.Validations
{
    public class CreateQrCodeTableValidator : AbstractValidator<QrCodeViewModel>
    {
        private readonly ValidationLocalizer _localizer;
        public CreateQrCodeTableValidator(ValidationLocalizer localizer)
        {
            _localizer = localizer;

            RuleFor(x => x.Name).NotEmpty().WithMessage(_localizer.GetLocalizedValue("TableName"))
                .MaximumLength(500).WithMessage(_localizer.GetLocalizedValue("MaxTableName"));

            RuleFor(x => x.TableNo).NotEmpty().WithMessage(_localizer.GetLocalizedValue("TableNo"))
                .GreaterThan(0).WithMessage(_localizer.GetLocalizedValue("MinTableNo"))
                .LessThan(10000).WithMessage(_localizer.GetLocalizedValue("MaxTableNo"))
                .Must(IsTableNoValid).WithMessage(_localizer.GetLocalizedValue("TableNoFormat"));
        }

        private bool IsTableNoValid(int tableNo)
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
    }
}
