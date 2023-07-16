using Frequency.Common.Shared;

namespace Frequency.Common.Entity;

public sealed class Category : GroupedEntity {
    public bool Default { get; set; } = false;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}