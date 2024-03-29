using Frequency.Common.Shared;

namespace Frequency.Common.Entity;

public sealed class Location : GroupedEntity {
    public bool Default { get; set; } = false;
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Address { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
}