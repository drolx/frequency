using System.ComponentModel.DataAnnotations;

namespace Proton.Frequency.Common.Entity;

public class BaseModel
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public Channel Channel { get; set; }
    public string? ChannelId { get; set; }
}
