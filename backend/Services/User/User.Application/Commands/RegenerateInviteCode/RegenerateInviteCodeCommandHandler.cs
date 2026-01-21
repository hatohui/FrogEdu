using FrogEdu.Shared.Kernel;
using FrogEdu.User.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FrogEdu.User.Application.Commands.RegenerateInviteCode;

/// <summary>
/// Handler for RegenerateInviteCodeCommand
/// </summary>
public sealed class RegenerateInviteCodeCommandHandler
    : IRequestHandler<RegenerateInviteCodeCommand, Result<string>>
{
    private readonly IClassRepository _classRepository;
    private readonly ILogger<RegenerateInviteCodeCommandHandler> _logger;

    public RegenerateInviteCodeCommandHandler(
        IClassRepository classRepository,
        ILogger<RegenerateInviteCodeCommandHandler> logger
    )
    {
        _classRepository = classRepository;
        _logger = logger;
    }

    public async Task<Result<string>> Handle(
        RegenerateInviteCodeCommand request,
        CancellationToken cancellationToken
    )
    {
        var classEntity = await _classRepository.GetByIdAsync(request.ClassId, cancellationToken);
        if (classEntity is null)
        {
            return Result<string>.Failure("Class not found");
        }

        // Verify teacher owns the class
        if (classEntity.HomeroomTeacherId != request.TeacherId)
        {
            _logger.LogWarning(
                "Unauthorized invite code regeneration attempt for class {ClassId} by {TeacherId}",
                request.ClassId,
                request.TeacherId
            );
            return Result<string>.Failure("You are not authorized to modify this class");
        }

        classEntity.GenerateNewInviteCode(request.ExpiresInDays);
        await _classRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Regenerated invite code for class {ClassId}", request.ClassId);

        return Result<string>.Success(classEntity.InviteCode!.Value);
    }
}
