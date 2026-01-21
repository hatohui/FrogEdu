using FrogEdu.Shared.Kernel;
using FrogEdu.User.Application.Interfaces;
using FrogEdu.User.Domain.Enums;
using FrogEdu.User.Domain.Repositories;
using MediatR;

namespace FrogEdu.User.Application.Commands.CreateUser;

/// <summary>
/// Handler for CreateUserCommand
/// </summary>
public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<Guid>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken
    )
    {
        // Check if user already exists
        var existingUser = await _userRepository.GetByCognitoIdAsync(
            request.CognitoId,
            cancellationToken
        );
        if (existingUser is not null)
        {
            return Result<Guid>.Failure("User with this Cognito ID already exists");
        }

        // Parse role
        if (!Enum.TryParse<UserRole>(request.Role, true, out var role))
        {
            role = UserRole.Student; // Default to Student if role is not provided or invalid
        }

        // Create user entity
        var user = Domain.Entities.User.Create(
            request.CognitoId,
            request.Email,
            request.FirstName,
            request.LastName,
            role
        );

        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(user.Id);
    }
}
