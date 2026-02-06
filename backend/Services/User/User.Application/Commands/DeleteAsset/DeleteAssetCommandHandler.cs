using FrogEdu.Shared.Kernel;
using FrogEdu.User.Domain.Services;
using MediatR;

namespace FrogEdu.User.Application.Commands.DeleteAsset;

public sealed class DeleteAssetCommandHandler(IAssetStorageService assetStorageService)
    : IRequestHandler<DeleteAssetCommand, Result>
{
    private readonly IAssetStorageService _assetStorageService = assetStorageService;

    public async Task<Result> Handle(
        DeleteAssetCommand request,
        CancellationToken cancellationToken
    )
    {
        var assetUrl = request.AssetUrl;

        await _assetStorageService.DeleteAssetAsync(assetUrl, cancellationToken);
        return Result.Success();
    }
}
