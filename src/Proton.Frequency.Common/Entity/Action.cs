using Proton.Frequency.Common.Enums;

namespace Proton.Frequency.Common.Entity;

public sealed class Action : BaseModel
{
    public DateTime Time { get; set; } = DateTime.Now;
    public bool Synced { get; set; } = false;
    public Terminal? Terminal { get; set; }
    public string? TerminalId { get; set; }
    public Object? Object { get; set; }
    public string? ObjectId { get; set; }
    public Node? Node { get; set; }
    public string? NodeId { get; set; }
}
