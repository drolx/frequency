using System.ComponentModel.DataAnnotations;

namespace Proton.Frequency.Model;

public class Terminal : CoreModel
{
    public string? UniqueId { get; set; }
    
    public string? Name { get; set; }

    public string? Mode { get; set; }

    public string? Protocol { get; set; }

    public DateTime TimeUpdated { get; set; } = DateTime.Now;

    public ICollection<Node>? Nodes { get; set; }

    public ICollection<Queue>? Queues { get; set; }
}
