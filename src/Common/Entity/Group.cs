using Frequency.Common.Shared;

namespace Frequency.Common.Entity;

public sealed class Group : TimedEntity {
    public bool Default { get; set; } = false;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Group? Parent { get; set; }
    public Guid? GroupId { get; set; }
    public ICollection<Group>? Children { get; set; }
}