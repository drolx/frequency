namespace Proton.Frequency.Common.Entity;

public sealed class Channel : Common.Entity
{
    public bool Default { get; set; } = false;
    public string Name { get; set; } = string.Empty;
    public string Identifier { get; set; } = string.Empty;
}
