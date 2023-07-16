namespace Frequency.Common.Shared;

public abstract class TimedEntity : BaseEntity {
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}