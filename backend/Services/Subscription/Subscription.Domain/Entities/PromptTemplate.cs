using FrogEdu.Shared.Kernel;
using FrogEdu.Shared.Kernel.Exceptions;

namespace FrogEdu.Subscription.Domain.Entities;

/// <summary>
/// PromptTemplate entity for managing AI prompt engineering templates
/// </summary>
public class PromptTemplate : Entity
{
    public string Name { get; private set; } = default!;
    public string Template { get; private set; } = default!;
    public string? Description { get; private set; }
    public int Version { get; private set; }
    public bool IsActive { get; private set; }

    private PromptTemplate() { } // For EF Core

    private PromptTemplate(string name, string template, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException(nameof(Name), "Template name cannot be empty");

        if (string.IsNullOrWhiteSpace(template))
            throw new ValidationException(nameof(Template), "Template content cannot be empty");

        Name = name;
        Template = template;
        Description = description;
        Version = 1;
        IsActive = true;
    }

    public static PromptTemplate Create(string name, string template, string? description = null)
    {
        return new PromptTemplate(name, template, description);
    }

    public void UpdateTemplate(string template, string? description)
    {
        if (string.IsNullOrWhiteSpace(template))
            throw new ValidationException(nameof(Template), "Template content cannot be empty");

        Template = template;
        Description = description;
        Version++;
        UpdateTimestamp();
    }

    public void Activate()
    {
        IsActive = true;
        UpdateTimestamp();
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdateTimestamp();
    }
}
