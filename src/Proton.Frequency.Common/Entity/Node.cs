namespace Proton.Frequency.Common.Entity;

public sealed class Node : BaseModel
{
    public string? UniqueId { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public Guid ReaderId { get; set; }
    public Terminal? Terminal { get; set; }
    public string? TerminalId { get; set; }
    public ICollection<Action>? Queues { get; set; }
}
