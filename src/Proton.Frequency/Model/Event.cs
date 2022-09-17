using Proton.Frequency.Enums;

namespace Proton.Frequency.Model;

public class Event : CoreModel
{
    public string? Type { get; set; }

    public string? Metadata { get; set; }
}
