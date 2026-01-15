using FrogEdu.AI.Domain.Entities;
using FrogEdu.Shared.Kernel;

namespace FrogEdu.AI.Domain.Interfaces;

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
