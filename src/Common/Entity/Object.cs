using Proton.Frequency.Common.Enums;
using Proton.Frequency.Common.Shared;

namespace Proton.Frequency.Common.Entity;

public sealed class Object : GroupedEntity {
    public string? Identifier { get; set; }
    public Guid? ObjectId { get; set; }
    public ICollection<Object>? Children { get; set; }
    public FrequencyType TagType { get; set; } = FrequencyType.Unspecified;
    public ObjectType Type { get; set; } = ObjectType.Unknown;
    public Category? Category { get; set; }
    public string? CategoryId { get; set; }
    public ObjectLog? Log { get; set; }
    public Guid? LogId { get; set; }
    public ICollection<ObjectLog>? Logs { get; set; }
    public Object? Parent { get; set; }
}

// TODO: Use var hierarchy = db.Hierarchy.Include(e => e.Children).ToList();
