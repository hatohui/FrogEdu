using FrogEdu.Shared.Kernel;
using FrogEdu.User.Application.Commands.DeleteAsset;
using FrogEdu.User.Application.Interfaces;
using FrogEdu.User.Domain.Repositories;
using MediatR;

namespace FrogEdu.User.Application.Commands.RemoveUserAvatar;

public sealed class RemoveUserAvatarCommandHandler
    : IRequestHandler<RemoveUserAvatarCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public RemoveUserAvatarCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IMediator mediator
    )
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }

    public async Task<Result> Handle(
        RemoveUserAvatarCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = await _userRepository.GetByCognitoIdAsync(request.CognitoId, cancellationToken);

        if (user is null)
        {
            return Result.Failure("User not found");
        }

        if (string.IsNullOrEmpty(user.AvatarUrl))
        {
            return Result.Success();
        }

        var deleteAssetCommand = new DeleteAssetCommand(user.AvatarUrl);
        var deleteResult = await _mediator.Send(deleteAssetCommand, cancellationToken);

        if (deleteResult.IsFailure)
        {
            return deleteResult;
        }

        user.RemoveAvatar();

        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
