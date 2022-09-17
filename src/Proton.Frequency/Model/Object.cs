using System.ComponentModel.DataAnnotations;
using Proton.Frequency.Enums;

namespace Proton.Frequency.Model;

public sealed class Object : CoreModel
{
    public string? UniqueId { get; set; }

    public ObjectType Type { get; set; } = ObjectType.VEHICLE;

    public ObjectTagType Tag { get; set; } = ObjectTagType.LF_KHZ;
    
    public DateTime TimeUpdated { get; set; } = DateTime.Now;

    public string? LastMode { get; set; }

    public ICollection<Queue>? Queues { get; set; }
}