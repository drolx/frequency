using System.ComponentModel.DataAnnotations;

namespace Proton.Frequency.Common.Entity;

public class CoreModel
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime TimeCreated { get; set; } = DateTime.Now;
}
