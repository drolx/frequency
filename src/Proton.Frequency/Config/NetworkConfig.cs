namespace Proton.Frequency.Config; 

public class NetworkConfig {
    public const string Key = "networks";
    public string Name { get; set; } = "device-0";
    public string Protocol { get; set; } = "chafon";
    public int Port { get; set; } = 1730;
}
