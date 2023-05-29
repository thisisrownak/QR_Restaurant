using FluentValidation;
using QR_Restaurant.UI.Helper;
using QR_Restaurant.UI.Models;

namespace QR_Restaurant.UI.Validations
{
    public class MenuFeaturesValidator : AbstractValidator<ProductFeatureViewModel>
    {
        private readonly ValidationLocalizer _localizer;
        public MenuFeaturesValidator(ValidationLocalizer localizer)
        {
            _localizer = localizer;

            RuleFor(x => x.Name).NotEmpty().WithMessage(_localizer.GetLocalizedValue("NotEmpty"))
                .MaximumLength(500).WithMessage(_localizer.GetLocalizedValue("MaxLength"));

            RuleFor(x => x.FeatureItemList).NotEmpty().When(x => x.Id == 0).WithMessage(_localizer.GetLocalizedValue("NotEmpty"))
              .MaximumLength(5000).WithMessage(_localizer.GetLocalizedValue("MaxLength"));

            RuleFor(x => x.MenuProductId).NotEqual(0).WithMessage(_localizer.GetLocalizedValue("NotEmpty"));
        }
    }
}