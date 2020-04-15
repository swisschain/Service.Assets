using Assets.Client.Models.AssetPairs;
using FluentValidation;

namespace Assets.WebApi.Validators
{
    public class AssetPairEditModelValidator : AbstractValidator<AssetPairEditModel>
    {
        public AssetPairEditModelValidator()
        {
            RuleFor(o => o.Symbol)
                .NotEmpty()
                .WithMessage("Symbol required.")
                .MaximumLength(36)
                .WithMessage("Symbol shouldn't be longer than 36 characters.");

            RuleFor(o => o.BaseAsset)
                .NotEmpty()
                .WithMessage("Base asset identifier required.")
                .MaximumLength(36)
                .WithMessage("Base asset shouldn't be longer than 36 characters.");

            RuleFor(o => o.QuotingAsset)
                .NotEmpty()
                .WithMessage("Quoting asset identifier required.")
                .MaximumLength(36)
                .WithMessage("Quoting asset shouldn't be longer than 36 characters.");

            RuleFor(o => o.Accuracy)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Accuracy should be greater than or equal to 0.");

            RuleFor(o => o.MinVolume)
                .GreaterThan(0m)
                .WithMessage("Min volume should be greater than 0.");

            RuleFor(o => o.MaxVolume)
                .GreaterThan(0m)
                .WithMessage("Max volume should be greater than 0.");

            RuleFor(o => o.MaxOppositeVolume)
                .GreaterThan(0m)
                .WithMessage("Max opposite volume should be greater than 0.");

            RuleFor(o => o.MarketOrderPriceThreshold)
                .GreaterThan(0m)
                .WithMessage("Market order price threshold should be greater than 0.");
        }
    }
}
