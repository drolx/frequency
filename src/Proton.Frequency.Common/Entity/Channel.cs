using System.ComponentModel.DataAnnotations;

namespace Proton.Frequency.Common.Entity; 

public sealed class Channel {
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string identifier { get; set; } = string.Empty;
}
