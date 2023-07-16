using Proton.Frequency.Common.Enums;
using Proton.Frequency.Common.Shared;

namespace Proton.Frequency.Common.Entity;

public sealed class Node : TimedEntity {
    public string? Identifier { get; set; }
    public bool Active { get; set; }
    public ActivityStatus Status { get; set; }
    public Channel Channel { get; set; } = null!;
    public Guid ChannelId { get; set; }
    public Terminal? Terminal { get; set; }
    public Guid? TerminalId { get; set; }
    public ICollection<ObjectLog>? Logs { get; set; }
    public string? Protocol { get; set; }

}