using Contracts.Common.Interfaces;
using Contracts.Domains.Interfaces;

namespace Contracts.Common.Events;

public class AudittableEventEntity<T>:EventEntity<T>, IAudittable, IEventEntity<T>
{
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset? LastModifiedDate { get; set; }
}