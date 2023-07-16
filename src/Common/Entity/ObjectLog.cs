using Proton.Frequency.Common.Shared;

namespace Proton.Frequency.Common.Entity;

public sealed class ObjectLog : TimedEntity {
    public DateTime Time { get; set; } = DateTime.Now;
    public bool Synchronized { get; set; } = false;
    public Terminal? Terminal { get; set; }
    public Guid? TerminalId { get; set; }
    public Object? Object { get; set; }
    public Guid? ObjectId { get; set; }
    public Node? Node { get; set; }
    public Guid? NodeId { get; set; }
}
