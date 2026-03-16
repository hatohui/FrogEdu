using FrogEdu.Class.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FrogEdu.Class.Application.Commands.AcceptReinvite;

public sealed class AcceptReinviteCommandHandler : IRequestHandler<AcceptReinviteCommand, bool>
{
    private readonly IClassEnrollmentRepository _enrollmentRepository;
    private readonly ILogger<AcceptReinviteCommandHandler> _logger;

    public AcceptReinviteCommandHandler(
        IClassEnrollmentRepository enrollmentRepository,
        ILogger<AcceptReinviteCommandHandler> logger
    )
    {
        _enrollmentRepository = enrollmentRepository;
        _logger = logger;
    }

    public async Task<bool> Handle(
        AcceptReinviteCommand request,
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
            enrollment.AcceptReinvite();
            _enrollmentRepository.Update(enrollment);
            await _enrollmentRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Student {StudentId} accepted reinvite to class {ClassId}",
                request.StudentId,
                request.ClassId
            );
            return true;
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(
                ex,
                "Failed to accept reinvite for student {StudentId}",
                request.StudentId
            );
            return false;
        }
    }
}
