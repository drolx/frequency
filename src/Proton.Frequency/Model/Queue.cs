using System.ComponentModel.DataAnnotations;
using Proton.Frequency.Enums;

namespace Proton.Frequency.Model;

public class Queue : CoreModel
{
    public DateTime Time { get; set; } = DateTime.Now;

    public int? Synced { get; set; } = 0;
    
    public EventType? Event { get; set;  }

    public Terminal? Terminal { get; set; }

    public Object? Object { get; set; }

    public Node? Node { get; set; }
}
