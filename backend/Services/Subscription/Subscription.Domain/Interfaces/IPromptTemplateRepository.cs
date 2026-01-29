using FrogEdu.Subscription.Domain.Entities;
using FrogEdu.Shared.Kernel;

namespace FrogEdu.Subscription.Domain.Interfaces;

/// <summary>
/// Repository interface for PromptTemplate aggregate
/// </summary>
public interface IPromptTemplateRepository : IRepository<PromptTemplate>
{
    Task<PromptTemplate?> GetByNameAsync(
        string name,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<PromptTemplate>> GetActiveTemplatesAsync(
        CancellationToken cancellationToken = default
    );
}

