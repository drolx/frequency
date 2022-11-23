using Proton.Frequency.Common.Entity;

namespace Proton.Frequency.Common.Shared;

public abstract class GroupedBaseEntity : TimedBaseEntity {
    public Group? Group { get; set; }
    public string? GroupId { get; set; }
}
