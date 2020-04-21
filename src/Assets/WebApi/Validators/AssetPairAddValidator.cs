using Assets.WebApi.Models.AssetPairs;
using FluentValidation;
using JetBrains.Annotations;

namespace Assets.WebApi.Validators
{
    [UsedImplicitly]
    public class AssetPairAddValidator : AbstractValidator<AssetPairAdd>
    {
        public AssetPairAddValidator()
        {
            RuleFor(o => o.Symbol)
                .NotEmpty()
                .WithMessage("Symbol required.")
                .MaximumLength(16)
                .WithMessage("Symbol must be less than 16 characters.");

            RuleFor(o => o.BaseAsset)
                .NotEmpty()
                .WithMessage("Base asset identifier required.")
                .MaximumLength(16)
                .WithMessage("Base asset must be less than 16 characters.");

            RuleFor(o => o.QuotingAsset)
                .NotEmpty()
                .WithMessage("Quoting asset identifier required.")
                .MaximumLength(16)
                .WithMessage("Quoting asset must be less than 16 characters.");

            RuleFor(o => o.Accuracy)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Accuracy must be greater than or equal to 0.");

            RuleFor(o => o.MinVolume)
                .GreaterThan(0m)
                .WithMessage("Min volume must be greater than 0.");

            RuleFor(o => o.MaxVolume)
                .GreaterThan(0m)
                .WithMessage("Max volume must be greater than 0.");

            RuleFor(o => o.MaxOppositeVolume)
                .GreaterThan(0m)
                .WithMessage("Max opposite volume must be greater than 0.");

            RuleFor(o => o.MarketOrderPriceThreshold)
                .GreaterThan(0m)
                .WithMessage("Market order price threshold must be greater than 0.");
        }
    }
}
