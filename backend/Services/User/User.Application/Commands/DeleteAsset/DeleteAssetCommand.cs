using FrogEdu.Shared.Kernel;
using MediatR;

namespace FrogEdu.User.Application.Commands.DeleteAsset;

public sealed record DeleteAssetCommand(string AssetUrl) : IRequest<Result>;
