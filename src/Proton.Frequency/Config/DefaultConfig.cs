namespace Proton.Frequency.Config;

public sealed class DefaultConfig
{
    public const string Key = "Service";
    public string Name { get; set; } = "UHF RFID";
    public bool Proxy { get; set; } = false;
    public bool Management { get; set; } = true;
    public bool Api { get; set; } = false;
    public bool Network { get; set; } = false;
    public string Secret { get; set; } = "secret";
}
