using Proton.Frequency.Common.Shared;

namespace Proton.Frequency.Common.Entity;

public sealed class Node : TimedBaseEntity {
    public string? Identifier { get; set; }
    public Channel Channel { get; set; } = null!;
    public Guid ChannelId { get; set; }
    public Terminal? Terminal { get; set; }
    public Guid? TerminalId { get; set; }
    public ICollection<Log>? Logs { get; set; }
    public string? Protocol { get; set; }
}
