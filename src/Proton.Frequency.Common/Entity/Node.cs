namespace Proton.Frequency.Common.Entity;

public sealed class Node : CoreModel
{
    public string? UniqueId { get; set; }

    public DateTime TimeUpdated { get; set; } = DateTime.Now;

    public Guid ReaderId { get; set; }

    public Terminal? Terminal { get; set; }

    public ICollection<Queue>? Queues { get; set; }
}
