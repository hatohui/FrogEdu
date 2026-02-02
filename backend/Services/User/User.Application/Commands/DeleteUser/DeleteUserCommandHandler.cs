using FrogEdu.Shared.Kernel;
using FrogEdu.User.Domain.Repositories;
using MediatR;

namespace FrogEdu.User.Application.Commands.DeleteUser;

public sealed class DeleteUserCommandHandler(IUserRepository userRepository)
    : IRequestHandler<DeleteUserCommand, Result>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure("User not found");
        }

        // Soft delete the user
        _userRepository.Delete(user);
        await _userRepository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
