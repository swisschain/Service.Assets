using Assets.Client.Models.Assets;
using FluentValidation;

namespace Assets.WebApi.Validators
{
    public class AssetEditModelValidator : AbstractValidator<AssetEditModel>
    {
        public AssetEditModelValidator()
        {
            RuleFor(o => o.Symbol)
                .NotEmpty()
                .WithMessage("Symbol required.")
                .MaximumLength(100)
                .WithMessage("Symbol shouldn't be longer than 100 characters.");

            RuleFor(o => o.Description)
                .MaximumLength(500)
                .WithMessage("Description shouldn't be longer than 500 characters.");

            RuleFor(o => o.Accuracy)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Accuracy should be greater than or equal to 0.");
        }
    }
}
