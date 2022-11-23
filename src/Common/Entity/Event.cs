using Proton.Frequency.Common.Enums;
using Proton.Frequency.Common.Shared;

namespace Proton.Frequency.Common.Entity;

public sealed class Event : GroupedBaseEntity {
    public EventType Type { get; set; } = EventType.UNKNOWN;
    public Log? Log { get; set; }
    public Guid? LogId { get; set; }
    public string? Metadata { get; set; }
}
