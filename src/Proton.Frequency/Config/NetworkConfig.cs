namespace Proton.Frequency.Config;

public class NetworkConfig : IConfig
{
    public static string Key { get; set; } = "networks";
    private static bool List { get; set; } = true;
    public string Identifier => Key;
    public bool IsList => List;
    public string Name { get; set; } = "device-0";
    public string Protocol { get; set; } = "chafon";
    public int Port { get; set; } = 1730;
}
