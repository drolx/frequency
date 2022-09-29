namespace Proton.Frequency.Services.ConfigOptions;

public class DefaultOptions
{
    public const string SectionKey = "App";

    public string Name { get; set; } = "UHF RFID";

    public bool Proxy { get; set; } = false;

    public bool Management { get; set; } = true;

    public bool Api { get; set; } = false;

    public string Secret { get; set; } = "secret";
}
