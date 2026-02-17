using FrogEdu.Class.Domain.Repositories;
using FrogEdu.Shared.Kernel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FrogEdu.Class.Application.Commands.DeleteExamSession;

public sealed class DeleteExamSessionCommandHandler
    : IRequestHandler<DeleteExamSessionCommand, Result>
{
    private readonly IExamSessionRepository _examSessionRepository;
    private readonly IClassRoomRepository _classRoomRepository;
    private readonly ILogger<DeleteExamSessionCommandHandler> _logger;

    public DeleteExamSessionCommandHandler(
        IExamSessionRepository examSessionRepository,
        IClassRoomRepository classRoomRepository,
        ILogger<DeleteExamSessionCommandHandler> logger
    )
    {
        _examSessionRepository = examSessionRepository;
        _classRoomRepository = classRoomRepository;
        _logger = logger;
    }

    public async Task<Result> Handle(
        DeleteExamSessionCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var session = await _examSessionRepository.GetByIdAsync(
                request.SessionId,
                cancellationToken
            );

            if (session is null)
                return Result.Failure("Exam session not found");

            if (!Guid.TryParse(request.UserId, out var userId))
                return Result.Failure("Invalid user ID format");

            var classroom = await _classRoomRepository.GetByIdAsync(
                session.ClassId,
                cancellationToken
            );

            if (classroom is null)
                return Result.Failure("Class not found");

            if (classroom.TeacherId != userId)
                return Result.Failure("Only the class teacher can delete exam sessions");

            _examSessionRepository.Delete(session);
            await _examSessionRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Exam session {SessionId} deleted by {UserId}",
                request.SessionId,
                userId
            );

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting exam session");
            return Result.Failure("An error occurred while deleting the exam session");
        }
    }
}
