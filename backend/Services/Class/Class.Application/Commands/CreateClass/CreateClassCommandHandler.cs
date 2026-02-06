using FrogEdu.Class.Application.Dtos;
using FrogEdu.Class.Domain.Entities;
using FrogEdu.Class.Domain.Repositories;
using FrogEdu.Shared.Kernel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FrogEdu.Class.Application.Commands.CreateClass;

public sealed class CreateClassCommandHandler
    : IRequestHandler<CreateClassCommand, Result<CreateClassResponse>>
{
    private readonly IClassRoomRepository _classRoomRepository;
    private readonly ILogger<CreateClassCommandHandler> _logger;

    public CreateClassCommandHandler(
        IClassRoomRepository classRoomRepository,
        ILogger<CreateClassCommandHandler> logger
    )
    {
        _classRoomRepository = classRoomRepository;
        _logger = logger;
    }

    public async Task<Result<CreateClassResponse>> Handle(
        CreateClassCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            // Parse userId to Guid for teacherId
            if (!Guid.TryParse(request.UserId, out var teacherId))
            {
                _logger.LogWarning(
                    "Invalid UserId format: {UserId}. Expected GUID.",
                    request.UserId
                );
                return Result<CreateClassResponse>.Failure("Invalid user ID format");
            }

            // Create the classroom entity
            var classroom = ClassRoom.Create(
                request.Name,
                request.Grade,
                request.MaxStudents,
                teacherId,
                request.UserId,
                request.BannerUrl
            );

            // Save to repository
            await _classRoomRepository.AddAsync(classroom, cancellationToken);
            await _classRoomRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Class created successfully: {ClassId} - {ClassName} by teacher {TeacherId}",
                classroom.Id,
                classroom.Name,
                teacherId
            );

            // Map to response
            var response = new CreateClassResponse(
                classroom.Id,
                classroom.Name,
                classroom.Grade,
                classroom.InviteCode.Value,
                classroom.MaxStudents,
                classroom.BannerUrl,
                classroom.IsActive,
                classroom.TeacherId,
                classroom.CreatedAt
            );

            return Result<CreateClassResponse>.Success(response);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Validation error while creating class");
            return Result<CreateClassResponse>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating class");
            return Result<CreateClassResponse>.Failure(
                "An error occurred while creating the class"
            );
        }
    }
}
