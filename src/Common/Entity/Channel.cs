using Frequency.Common.Shared;

namespace Frequency.Common.Entity;

public sealed class Channel : BaseEntity {
    public bool Default { get; set; } = false;
    public string Name { get; set; } = string.Empty;
    public string Identifier { get; set; } = string.Empty;
}