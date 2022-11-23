using Proton.Frequency.Common.Enums;
using Proton.Frequency.Common.Shared;

namespace Proton.Frequency.Common.Entity;

public sealed class ObjectEvent : GroupedEntity {
    public EventType Type { get; set; } = EventType.Unknown;
    public ObjectLog? Log { get; set; }
    public Guid? LogId { get; set; }
    public string? Metadata { get; set; }
}
