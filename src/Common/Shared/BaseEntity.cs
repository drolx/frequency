using System.ComponentModel.DataAnnotations;

namespace Frequency.Common.Shared;

public abstract class BaseEntity {
    [Key] public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime ArchivedAt { get; set; }
}