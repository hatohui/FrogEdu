using System;

namespace FrogEdu.Shared.Kernel.Auditing;

public interface IAuditable
{
    DateTime CreatedAt { get; }
    Guid? CreatedBy { get; }
    DateTime? UpdatedAt { get; }
    Guid? UpdatedBy { get; }
}
