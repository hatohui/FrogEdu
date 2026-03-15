using FrogEdu.Shared.Kernel.Primitives;

namespace FrogEdu.Class.Domain.Entities;

public sealed class Badge : Entity
{
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string IconUrl { get; private set; } = null!;
    public string BadgeType { get; private set; } = null!; // "cup", "medal", "star", "frog"
    public int RequiredScore { get; private set; }
    public bool IsActive { get; private set; }

    private Badge() { }

    private Badge(
        string name,
        string description,
        string iconUrl,
        string badgeType,
        int requiredScore
    )
    {
        Name = name;
        Description = description;
        IconUrl = iconUrl;
        BadgeType = badgeType;
        RequiredScore = requiredScore;
        IsActive = true;
    }

    public static Badge Create(
        string name,
        string description,
        string iconUrl,
        string badgeType,
        int requiredScore
    )
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Badge name cannot be empty", nameof(name));
        if (string.IsNullOrWhiteSpace(badgeType))
            throw new ArgumentException("Badge type cannot be empty", nameof(badgeType));

        return new Badge(name, description, iconUrl, badgeType, requiredScore);
    }

    public void Update(
        string name,
        string description,
        string iconUrl,
        string badgeType,
        int requiredScore
    )
    {
        Name = name;
        Description = description;
        IconUrl = iconUrl;
        BadgeType = badgeType;
        RequiredScore = requiredScore;
    }

    public void Activate() => IsActive = true;

    public void Deactivate() => IsActive = false;
}
