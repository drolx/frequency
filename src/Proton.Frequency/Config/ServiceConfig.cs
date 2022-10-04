namespace Proton.Frequency.Config;

public sealed class ServiceConfig : IConfig
{
    public const string Key = "service";
    public const bool IsList = false;
    public string Name { get; set; } = "RFID Service";
    public bool Proxy { get; set; } = false;
    public bool Management { get; set; } = true;
    public bool Api { get; set; } = false;
    public bool Network { get; set; } = false;
    public string Secret { get; set; } = "secret";
}
