using Proton.Frequency.Common.Enums;

namespace Proton.Frequency.Common.Dto; 

public class EventDto {
    public EventType Type { get; set; } = EventType.UNKNOWN;
}
