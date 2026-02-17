using FrogEdu.Class.Application.Dtos;
using FrogEdu.Class.Domain.Repositories;
using FrogEdu.Shared.Kernel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FrogEdu.Class.Application.Commands.JoinClass;

public sealed class JoinClassCommandHandler
    : IRequestHandler<JoinClassCommand, Result<JoinClassResponse>>
{
    private readonly IClassRoomRepository _classRoomRepository;
    private readonly ILogger<JoinClassCommandHandler> _logger;

    public JoinClassCommandHandler(
        IClassRoomRepository classRoomRepository,
        ILogger<JoinClassCommandHandler> logger
    )
    {
        _classRoomRepository = classRoomRepository;
        _logger = logger;
    }

    public async Task<Result<JoinClassResponse>> Handle(
        JoinClassCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var normalizedCode = request.InviteCode.Trim().ToUpperInvariant();

            var classroom = await _classRoomRepository.GetByInviteCodeAsync(
                normalizedCode,
                cancellationToken
            );

            if (classroom is null)
            {
                return Result<JoinClassResponse>.Failure("Invalid invite code");
            }

            if (!Guid.TryParse(request.UserId, out var studentId))
            {
                return Result<JoinClassResponse>.Failure("Invalid user ID format");
            }

            // EnrollStudent validates capacity and duplicate enrollment
            classroom.EnrollStudent(studentId);

            _classRoomRepository.Update(classroom);
            await _classRoomRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Student {StudentId} joined class {ClassId} ({ClassName}) via invite code",
                studentId,
                classroom.Id,
                classroom.Name
            );

            return Result<JoinClassResponse>.Success(
                new JoinClassResponse(classroom.Id, classroom.Name)
            );
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Cannot join class: {Message}", ex.Message);
            return Result<JoinClassResponse>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while joining class");
            return Result<JoinClassResponse>.Failure("An error occurred while joining the class");
        }
    }
}
