using Assets.WebApi.Models.Assets;
using FluentValidation;
using JetBrains.Annotations;

namespace Assets.WebApi.Validators
{
    [UsedImplicitly]
    public class AssetRequestManyValidator : AbstractValidator<AssetRequestMany>
    {
        public AssetRequestManyValidator()
        {
            RuleFor(o => o.Limit)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Limit must be greater or equal then 0.")
                .LessThanOrEqualTo(1000)
                .WithMessage("Limit must be less or equal to 1000.");
        }
    }
}
