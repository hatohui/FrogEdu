using FrogEdu.Shared.Kernel.Auditing;

namespace FrogEdu.Shared.Kernel.Primitives;

public abstract class AuditableEntity : Entity, IAuditable
{
    public DateTime CreatedAt { get; protected set; }
    public Guid? CreatedBy { get; protected set; }

    public DateTime? UpdatedAt { get; protected set; }

    public Guid? UpdatedBy { get; protected set; }

    protected AuditableEntity(Guid? createdBy = null)
    {
        CreatedAt = DateTime.UtcNow;
        CreatedBy = createdBy;
    }

    protected void MarkAsCreated(Guid? createdBy = null)
    {
        CreatedAt = DateTime.UtcNow;
        CreatedBy = createdBy;
    }

    protected void MarkAsUpdated(Guid? updatedBy = null)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }
}
