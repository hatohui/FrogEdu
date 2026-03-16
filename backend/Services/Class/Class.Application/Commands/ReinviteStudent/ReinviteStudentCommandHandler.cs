using FrogEdu.Class.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FrogEdu.Class.Application.Commands.ReinviteStudent;

public sealed class ReinviteStudentCommandHandler : IRequestHandler<ReinviteStudentCommand, bool>
{
    private readonly IClassEnrollmentRepository _enrollmentRepository;
    private readonly ILogger<ReinviteStudentCommandHandler> _logger;

    public ReinviteStudentCommandHandler(
        IClassEnrollmentRepository enrollmentRepository,
        ILogger<ReinviteStudentCommandHandler> logger
    )
    {
        _enrollmentRepository = enrollmentRepository;
        _logger = logger;
    }

    public async Task<bool> Handle(
        ReinviteStudentCommand request,
        CancellationToken cancellationToken
    )
    {
        var enrollment = await _enrollmentRepository.GetByClassAndStudentAsync(
            request.ClassId,
            request.StudentId,
            cancellationToken
        );

        if (enrollment is null)
        {
            _logger.LogWarning(
                "Enrollment for student {StudentId} in class {ClassId} not found",
                request.StudentId,
                request.ClassId
            );
            return false;
        }

        try
        {
            enrollment.Reinvite();
            _enrollmentRepository.Update(enrollment);
            await _enrollmentRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Student {StudentId} reinvited to class {ClassId}",
                request.StudentId,
                request.ClassId
            );
            return true;
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Failed to reinvite student {StudentId}", request.StudentId);
            return false;
        }
    }
}
