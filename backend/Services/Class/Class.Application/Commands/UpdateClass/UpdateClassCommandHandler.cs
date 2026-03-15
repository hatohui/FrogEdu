using FrogEdu.Class.Domain.Repositories;
using FrogEdu.Shared.Kernel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FrogEdu.Class.Application.Commands.UpdateClass;

public sealed class UpdateClassCommandHandler : IRequestHandler<UpdateClassCommand, Result>
{
    private readonly IClassRoomRepository _classRoomRepository;
    private readonly ILogger<UpdateClassCommandHandler> _logger;

    public UpdateClassCommandHandler(
        IClassRoomRepository classRoomRepository,
        ILogger<UpdateClassCommandHandler> logger
    )
    {
        _classRoomRepository = classRoomRepository;
        _logger = logger;
    }

    public async Task<Result> Handle(
        UpdateClassCommand request,
        CancellationToken cancellationToken
    )
    {
        var classroom = await _classRoomRepository.GetByIdAsync(request.ClassId, cancellationToken);

        if (classroom is null)
        {
            _logger.LogWarning("Class not found: {ClassId}", request.ClassId);
            return Result.Failure("Class not found");
        }

        if (!Guid.TryParse(request.UserId, out var userGuid))
        {
            return Result.Failure("Invalid user ID format");
        }

        if (classroom.TeacherId != userGuid)
        {
            _logger.LogWarning(
                "User {UserId} is not the teacher of class {ClassId}",
                request.UserId,
                request.ClassId
            );
            return Result.Failure("Unauthorized to update this class");
        }

        classroom.Update(
            request.Name,
            request.Grade,
            request.MaxStudents,
            request.BannerUrl,
            request.UserId
        );

        _classRoomRepository.Update(classroom);
        await _classRoomRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Class updated: {ClassId}", request.ClassId);
        return Result.Success();
    }
}
