using Proton.Frequency.Common.Entity;

namespace Proton.Frequency.Common.Common;

public abstract class GroupedEntity : TimedEntity {
    public Group? Group { get; set; }
    public string? GroupId { get; set; }
}
