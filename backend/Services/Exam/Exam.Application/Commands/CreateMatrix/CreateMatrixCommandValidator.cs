using FluentValidation;

namespace FrogEdu.Exam.Application.Commands.CreateMatrix;

public sealed class CreateMatrixCommandValidator : AbstractValidator<CreateMatrixCommand>
{
    public CreateMatrixCommandValidator()
    {
        RuleFor(x => x.ExamId).NotEmpty().WithMessage("Exam ID is required");

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
