using Proton.Frequency.Common.Enums;

namespace Proton.Frequency.Common.Entity;

public class Queue : CoreModel
{
    public DateTime Time { get; set; } = DateTime.Now;

    public int? Synced { get; set; } = 0;

    public EventType? Event { get; set; }

    public Terminal? Terminal { get; set; }

    public Object? Object { get; set; }

    public Node? Node { get; set; }
}
