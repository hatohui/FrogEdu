using FrogEdu.Shared.Kernel;
using FrogEdu.User.Application.Interfaces;
using FrogEdu.User.Domain.Entities;
using FrogEdu.User.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FrogEdu.User.Application.Commands.CreateClass;

/// <summary>
/// Handler for CreateClassCommand
/// </summary>
public sealed class CreateClassCommandHandler : IRequestHandler<CreateClassCommand, Result<Guid>>
{
    private readonly IClassRepository _classRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<CreateClassCommandHandler> _logger;

    public CreateClassCommandHandler(
        IClassRepository classRepository,
        IUserRepository userRepository,
        ILogger<CreateClassCommandHandler> logger
    )
    {
        _classRepository = classRepository;
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(
        CreateClassCommand request,
        CancellationToken cancellationToken
    )
    {
        // Verify teacher exists
        var teacher = await _userRepository.GetByIdAsync(request.TeacherId, cancellationToken);
        if (teacher is null)
        {
            _logger.LogWarning(
                "Attempted to create class for non-existent teacher: {TeacherId}",
                request.TeacherId
            );
            return Result<Guid>.Failure("Teacher not found");
        }

        // Create class entity
        var classEntity = new Class(
            request.Name,
            request.Grade,
            request.TeacherId,
            request.Subject,
            request.School,
            request.Description,
            request.MaxStudents
        );

        await _classRepository.AddAsync(classEntity, cancellationToken);
        await _classRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Created class {ClassId} for teacher {TeacherId}",
            classEntity.Id,
            request.TeacherId
        );

        return Result<Guid>.Success(classEntity.Id);
    }
}
