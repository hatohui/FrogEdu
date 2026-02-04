using FluentValidation;

namespace FrogEdu.Exam.Application.Commands.CreateMatrix;

public sealed class CreateMatrixCommandValidator : AbstractValidator<CreateMatrixCommand>
{
    public CreateMatrixCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Matrix name is required")
            .MaximumLength(200)
            .WithMessage("Matrix name cannot exceed 200 characters");

        RuleFor(x => x.SubjectId).NotEmpty().WithMessage("Subject ID is required");

        RuleFor(x => x.Grade).InclusiveBetween(1, 12).WithMessage("Grade must be between 1 and 12");

        RuleFor(x => x.MatrixTopics)
            .NotEmpty()
            .WithMessage("At least one matrix topic is required");

        RuleForEach(x => x.MatrixTopics)
            .ChildRules(topic =>
            {
                topic.RuleFor(x => x.TopicId).NotEmpty().WithMessage("Topic ID is required");

                topic
                    .RuleFor(x => x.Quantity)
                    .GreaterThan(0)
                    .WithMessage("Quantity must be greater than 0")
                    .LessThanOrEqualTo(50)
                    .WithMessage("Quantity cannot exceed 50 per topic/level");
            });
    }
}
