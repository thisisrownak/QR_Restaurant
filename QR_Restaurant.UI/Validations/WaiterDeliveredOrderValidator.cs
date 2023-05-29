using FluentValidation;
using QR_Restaurant.UI.Helper;
using QR_Restaurant.UI.Models;

namespace QR_Restaurant.UI.Validations
{
    public class WaiterDeliveredOrderValidator : AbstractValidator<DeliveredOrdersViewModel>
    {
        private readonly ValidationLocalizer _localizer;
        public WaiterDeliveredOrderValidator(ValidationLocalizer localizer)
        {
            _localizer = localizer;

            RuleFor(x => x.EndDate)
              .GreaterThan(x => x.StartDate).WithMessage(_localizer.GetLocalizedValue("StartEndDate"));
        }
    }
}
