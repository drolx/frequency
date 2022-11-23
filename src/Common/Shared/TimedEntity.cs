namespace Proton.Frequency.Common.Shared;

public abstract class TimedBaseEntity : BaseEntity {
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
