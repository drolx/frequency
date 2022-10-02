namespace Proton.Frequency.Config;

public sealed class DefaultConfig : IConfig
{
    public static string Key { get; set; } = "service";
    private static bool List { get; set; } = false;
    public string Identifier => Key;
    public bool IsList => List;
    public string Name { get; set; } = "UHF RFID";
    public bool Proxy { get; set; } = false;
    public bool Management { get; set; } = true;
    public bool Api { get; set; } = false;
    public bool Network { get; set; } = false;
    public string Secret { get; set; } = "secret";
}
