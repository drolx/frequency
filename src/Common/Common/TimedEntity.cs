namespace Proton.Frequency.Common.Common;

public abstract class TimedEntity : Entity
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
