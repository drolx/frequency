using Proton.Frequency.Common.Common;
using Proton.Frequency.Common.Enums;

namespace Proton.Frequency.Common.Entity;

public sealed class Object : GroupedEntity
{
    public string? Identifier { get; set; }
    public Guid? ObjectId { get; set; }
    public ICollection<Object>? Children { get; set; }
    public ObjectTagType TagType { get; set; } = ObjectTagType.UNSPECIFIED;
    public ObjectType Type { get; set; } = ObjectType.VEHICLE;
    public Category? Category { get; set; }
    public string? CategoryId { get; set; }
    public Log? Log { get; set; }
    public Guid? LogId { get; set; }
    public ICollection<Log>? Logs { get; set; }
    public Object? Parent { get; set; }
    
}

// TODO: Use var hierarchy = db.Hierarchy.Include(e => e.Children).ToList();
