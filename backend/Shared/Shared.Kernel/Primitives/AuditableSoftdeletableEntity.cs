using FrogEdu.Shared.Kernel.Deletion;

namespace FrogEdu.Shared.Kernel.Primitives;

public abstract class AuditableSoftdeletableEntity : AuditableEntity, ISoftDeletable
{
    public bool IsDeleted { get; protected set; }
    public DateTime? DeletedAt { get; protected set; }
    public Guid? DeletedBy { get; protected set; }

    protected AuditableSoftdeletableEntity(Guid? createdBy = null)
        : base(createdBy)
    {
        IsDeleted = false;
    }

    public void MarkAsDeleted(Guid? deletedBy = null)
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;
    }

    public void Restore()
    {
        IsDeleted = false;
        DeletedAt = null;
        DeletedBy = null;
    }
}
