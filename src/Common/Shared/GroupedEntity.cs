using Frequency.Common.Entity;

namespace Frequency.Common.Shared;

public abstract class GroupedEntity : TimedEntity {
    public Group? Group { get; set; }
    public string? GroupId { get; set; }
}