using FluentValidation;
using QR_Restaurant.UI.Helper;
using QR_Restaurant.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QR_Restaurant.UI.Validations
{
    public class RestaurantSoldStockReportValidator : AbstractValidator<SoldStocksViewModel>
    {
        private readonly ValidationLocalizer _localizer;
        public RestaurantSoldStockReportValidator(ValidationLocalizer localizer)
        {
            _localizer = localizer;

            RuleFor(x => x.EndDate)
              .GreaterThan(x => x.StartDate).WithMessage(_localizer.GetLocalizedValue("StartEndDate"));
        }
    }
}
