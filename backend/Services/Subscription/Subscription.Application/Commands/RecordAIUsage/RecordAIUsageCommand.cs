using FrogEdu.Shared.Kernel;
using MediatR;

namespace FrogEdu.Subscription.Application.Commands.RecordAIUsage;

public sealed record RecordAIUsageCommand(Guid UserId, string ActionType, string? Metadata = null) : IRequest<Result<Guid>>;
