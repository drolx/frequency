using Proton.Frequency.Common.Shared;

namespace Proton.Frequency.Common.Entity;

public sealed class Category : GroupedBaseEntity {
    public bool Default { get; set; } = false;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
