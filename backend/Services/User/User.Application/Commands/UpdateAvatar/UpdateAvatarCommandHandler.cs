using FrogEdu.Shared.Kernel;
using FrogEdu.User.Application.Interfaces;
using FrogEdu.User.Domain.Repositories;
using MediatR;

namespace FrogEdu.User.Application.Commands.UpdateAvatar;

/// <summary>
/// Handler for UpdateAvatarCommand
/// </summary>
public sealed class UpdateAvatarCommandHandler : IRequestHandler<UpdateAvatarCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAvatarCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        UpdateAvatarCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = await _userRepository.GetByCognitoIdAsync(request.CognitoId, cancellationToken);

        if (user is null)
        {
            return Result.Failure("User not found");
        }

        user.UpdateAvatar(request.AvatarUrl);

        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
