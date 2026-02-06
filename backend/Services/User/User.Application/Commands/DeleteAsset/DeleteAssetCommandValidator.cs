using FluentValidation;

namespace FrogEdu.User.Application.Commands.DeleteAsset;

public sealed class DeleteAssetCommandValidator : AbstractValidator<DeleteAssetCommand>
{
    public DeleteAssetCommandValidator()
    {
        RuleFor(x => x.AssetUrl).NotEmpty().WithMessage("Asset URL is required");
    }
}
