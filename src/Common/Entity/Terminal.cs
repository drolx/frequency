using Frequency.Common.Enums;
using Frequency.Common.Shared;

namespace Frequency.Common.Entity;

public sealed class Terminal : GroupedEntity {
    public string? Identifier { get; set; }
    public string? Name { get; set; }
    public bool Active { get; set; }
    public ActivityStatus Status { get; set; }
    public Channel Channel { get; set; } = null!;
    public Guid ChannelId { get; set; }
    public Location? Location { get; set; }
    public Guid? LocationId { get; set; }
    public bool Proxy { get; set; } = true;
    public ICollection<Node>? Nodes { get; set; }
    public ICollection<ObjectLog>? Logs { get; set; }
}