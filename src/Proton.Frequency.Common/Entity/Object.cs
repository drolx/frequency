using Proton.Frequency.Common.Enums;

namespace Proton.Frequency.Common.Entity;

public sealed class Object : CoreModel
{
    public string? TagId { get; set; }

    public ObjectTagType TagType { get; set; } = ObjectTagType.LF_KHZ;

    public ObjectType Type { get; set; } = ObjectType.VEHICLE;

    public DateTime TimeUpdated { get; set; } = DateTime.Now;

    public string? LastMode { get; set; }

    public ICollection<Queue>? Queues { get; set; }

    public Object? Parent { get; set; }

    public ICollection<Object>? Children { get; set; }
}

// var hierarchy = db.Hierarchy.Include(e => e.Children).ToList();
