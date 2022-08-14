using Contracts.Domains.Interfaces;

namespace Contracts.Domains;

public abstract class EntityAuditBase<T>: EntityBase<T>, IAudittable
{
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset? LastModifiedDate { get; set; }
}