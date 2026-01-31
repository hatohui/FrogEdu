using System;

namespace FrogEdu.Shared.Kernel.Deletion;

public interface ISoftDeletable
{
    bool IsDeleted { get; }
    DateTime? DeletedAt { get; }
    Guid? DeletedBy { get; }
    void MarkAsDeleted(Guid? deletedBy);
    void Restore();
}
