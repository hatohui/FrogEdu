using FrogEdu.Shared.Kernel;
using FrogEdu.User.Application.Interfaces;
using FrogEdu.User.Domain.Enums;
using FrogEdu.User.Domain.Repositories;
using MediatR;

namespace FrogEdu.User.Application.Commands.CreateUser;

public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<Guid>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRoleService _roleService;

    public CreateUserCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IRoleService roleService
    )
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _roleService = roleService;
    }

    public async Task<Result<Guid>> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var existingUser = await _userRepository.GetByCognitoIdAsync(
            request.CognitoId,
            cancellationToken
        );
        if (existingUser is not null)
        {
            return Result<Guid>.Failure("User with this Cognito ID already exists");
        }

        UserRole parsedRole;
        if (
            string.IsNullOrWhiteSpace(request.Role)
            || !Enum.TryParse<UserRole>(request.Role, true, out parsedRole)
        )
        {
            parsedRole = UserRole.Student;
        }

        // Resolve role from DB by name
        var roleName = parsedRole.ToString();
        var roleDto = await _roleService.GetRoleByNameAsync(roleName, cancellationToken);

        // If role not found in DB, try fallback to Student seeded role
        if (roleDto is null)
        {
            roleDto = await _roleService.GetRoleByNameAsync(
                UserRole.Student.ToString(),
                cancellationToken
            );
            if (roleDto is null)
                return Result<Guid>.Failure("Role configuration is missing");
        }

        var user = Domain.Entities.User.Create(
            request.CognitoId,
            request.Email,
            request.FirstName,
            request.LastName,
            roleDto.Id,
            request.AvatarUrl
        );

        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(user.Id);
    }
}
