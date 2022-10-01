using Proton.Frequency.Common.Enums;

namespace Proton.Frequency.Common.Entity;

public sealed class Object : BaseModel
{
    public string? TagId { get; set; }
    public Category? Category { get; set; }
    public ObjectTagType TagType { get; set; } = ObjectTagType.LF_KHZ;
    public ObjectType Type { get; set; } = ObjectType.VEHICLE;
    public DateTime? UpdatedAt { get; set; }
    public Action? Action { get; set; }
    public string? ActionId { get; set; }
    public Event? Event { get; set; }
    public string? EventId { get; set; }
    public ICollection<Action>? Actions { get; set; }
    public Object? Parent { get; set; }
    public string? ObjectId { get; set; }
    public ICollection<Object>? Children { get; set; }
}

// TODO: Use var hierarchy = db.Hierarchy.Include(e => e.Children).ToList();
