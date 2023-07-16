using Proton.Frequency.Common.Entity;

namespace Proton.Frequency.Common.Shared;

public abstract class GroupedEntity : TimedEntity {
    public Group? Group { get; set; }
    public string? GroupId { get; set; }
}
