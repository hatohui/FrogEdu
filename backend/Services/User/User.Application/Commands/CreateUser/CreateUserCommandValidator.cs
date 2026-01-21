using FluentValidation;

namespace FrogEdu.User.Application.Commands.CreateUser;

/// <summary>
/// Validator for CreateUserCommand
/// </summary>
public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.CognitoId)
            .NotEmpty()
            .WithMessage("Cognito ID is required")
            .MaximumLength(100)
            .WithMessage("Cognito ID must not exceed 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Invalid email format")
            .MaximumLength(200)
            .WithMessage("Email must not exceed 200 characters");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required")
            .MaximumLength(100)
            .WithMessage("First name must not exceed 100 characters");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required")
            .MaximumLength(100)
            .WithMessage("Last name must not exceed 100 characters");

        RuleFor(x => x.Role)
            .NotEmpty()
            .WithMessage("Role is required")
            .Must(r =>
                r.Equals("Student", StringComparison.OrdinalIgnoreCase)
                || r.Equals("Teacher", StringComparison.OrdinalIgnoreCase)
            )
            .WithMessage("Role must be either 'Student' or 'Teacher'");
    }
}
