using System.ComponentModel.DataAnnotations;

namespace Proton.Frequency.Common.Common;

public abstract class Entity {
    [Key] public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime ArchivedAt { get; set; }
}
