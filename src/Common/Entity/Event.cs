using Proton.Frequency.Common.Common;
using Proton.Frequency.Common.Enums;

namespace Proton.Frequency.Common.Entity;

public sealed class Event : GroupedEntity {
    public EventType Type { get; set; } = EventType.UNKNOWN;
    public Log? Log { get; set; }
    public Guid? LogId { get; set; }
    public string? Metadata { get; set; }
}
