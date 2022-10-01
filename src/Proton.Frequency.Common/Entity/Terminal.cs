using System.ComponentModel.DataAnnotations;

namespace Proton.Frequency.Common.Entity;

public sealed class Terminal : BaseModel
{
    public string? UniqueId { get; set; }
    public string? Name { get; set; }
    public bool Proxy { get; set; } = true;
    public string? Protocol { get; set; }
    public DateTime TimeUpdated { get; set; } = DateTime.Now;
    public ICollection<Node>? Nodes { get; set; }
    public ICollection<Action>? Queues { get; set; }
}
