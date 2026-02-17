using FrogEdu.Class.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FrogEdu.Class.Application.Commands.RemoveStudent;

public sealed class RemoveStudentCommandHandler : IRequestHandler<RemoveStudentCommand, bool>
{
    private readonly IClassRoomRepository _classRoomRepository;
    private readonly ILogger<RemoveStudentCommandHandler> _logger;

    public RemoveStudentCommandHandler(
        IClassRoomRepository classRoomRepository,
        ILogger<RemoveStudentCommandHandler> logger
    )
    {
        _classRoomRepository = classRoomRepository;
        _logger = logger;
    }

    public async Task<bool> Handle(
        RemoveStudentCommand request,
        CancellationToken cancellationToken
    )
    {
        var classroom = await _classRoomRepository.GetByIdAsync(request.ClassId, cancellationToken);

        if (classroom is null)
        {
            _logger.LogWarning("Classroom {ClassId} not found", request.ClassId);
            return false;
        }

        try
        {
            classroom.RemoveStudent(request.StudentId);
            _classRoomRepository.Update(classroom);
            await _classRoomRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Student {StudentId} removed from classroom {ClassId}",
                request.StudentId,
                request.ClassId
            );

            return true;
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(
                ex,
                "Failed to remove student {StudentId} from classroom {ClassId}",
                request.StudentId,
                request.ClassId
            );
            return false;
        }
    }
}
