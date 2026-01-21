using FrogEdu.Shared.Kernel;
using FrogEdu.User.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FrogEdu.User.Application.Commands.JoinClass;

/// <summary>
/// Handler for JoinClassCommand
/// </summary>
public sealed class JoinClassCommandHandler : IRequestHandler<JoinClassCommand, Result<Guid>>
{
    private readonly IClassRepository _classRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<JoinClassCommandHandler> _logger;

    public JoinClassCommandHandler(
        IClassRepository classRepository,
        IUserRepository userRepository,
        ILogger<JoinClassCommandHandler> logger
    )
    {
        _classRepository = classRepository;
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(
        JoinClassCommand request,
        CancellationToken cancellationToken
    )
    {
        // Normalize invite code
        var normalizedCode = request.InviteCode.ToUpperInvariant().Trim();

        // Find class by invite code
        var classEntity = await _classRepository.GetByInviteCodeAsync(
            normalizedCode,
            cancellationToken
        );
        if (classEntity is null)
        {
            _logger.LogWarning("Invalid invite code attempted: {InviteCode}", normalizedCode);
            return Result<Guid>.Failure("Invalid or expired invite code");
        }

        // Validate the invite code
        if (!classEntity.ValidateInviteCode(normalizedCode))
        {
            _logger.LogWarning("Expired invite code attempted for class {ClassId}", classEntity.Id);
            return Result<Guid>.Failure("Invalid or expired invite code");
        }

        // Check if class is archived
        if (classEntity.IsArchived)
        {
            return Result<Guid>.Failure("This class is no longer accepting students");
        }

        // Verify student exists
        var student = await _userRepository.GetByIdAsync(request.StudentId, cancellationToken);
        if (student is null)
        {
            _logger.LogWarning(
                "Attempted to join class with non-existent student: {StudentId}",
                request.StudentId
            );
            return Result<Guid>.Failure("Student not found");
        }

        try
        {
            // Add student to class
            classEntity.AddStudent(request.StudentId);
            await _classRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Student {StudentId} joined class {ClassId}",
                request.StudentId,
                classEntity.Id
            );

            return Result<Guid>.Success(classEntity.Id);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(
                ex,
                "Failed to add student {StudentId} to class {ClassId}",
                request.StudentId,
                classEntity.Id
            );
            return Result<Guid>.Failure(ex.Message);
        }
    }
}
