using Proton.Frequency.Common.Enums;

namespace Proton.Frequency.Common.Entity;

public sealed class Event : BaseModel {
    public EventType Type { get; set; } = EventType.UNKNOWN;
    public Action? Action { get; set; }
    public string? ActionId { get; set; }
    public string? Metadata { get; set; }
}
