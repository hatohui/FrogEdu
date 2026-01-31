using System;
using FrogEdu.Shared.Kernel.Primitives;
using FrogEdu.User.Domain.Enums;

namespace FrogEdu.User.Domain.Entities;

public sealed class Role : Entity
{
    public UserRole Name { get; private set; }
    public string Description { get; private set; } = string.Empty;

    private Role() { }

    private Role(UserRole name, string description)
    {
        if (!Enum.IsDefined(name))
            throw new ArgumentException("Invalid role value", nameof(name));
        else
        {
            Name = name;
            Description = description?.Trim() ?? string.Empty;
        }
    }

    public static Role Create(UserRole name, string description = "")
    {
        return new Role(name, description);
    }

    public void Update(UserRole name, string description)
    {
        if (!Enum.IsDefined(name))
            throw new ArgumentException("Invalid role value", nameof(name));

        Name = name;
        Description = description?.Trim() ?? string.Empty;
    }
}
