using Frequency.Common.Enums;
using Frequency.Common.Shared;

namespace Frequency.Common.Entity;

public sealed class ObjectEvent : GroupedEntity {
    public EventType Type { get; set; } = EventType.Unknown;
    public ObjectLog? Log { get; set; }
    public Guid? LogId { get; set; }
    public string? Metadata { get; set; }
}