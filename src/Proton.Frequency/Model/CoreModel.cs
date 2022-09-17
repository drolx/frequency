using System.ComponentModel.DataAnnotations;

namespace Proton.Frequency.Model;

public class CoreModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public DateTime TimeCreated { get; set; } = DateTime.Now;
}
