using Proton.Frequency.Common.Interfaces;

namespace Proton.Frequency.Common.Config;

public class NetworkConfig : IConfig {
    public const string Key = "networks";
    public string Name { get; set; } = "device-0";
    public string Protocol { get; set; } = "chafon";
    public int Port { get; set; } = 1730;
    public bool Server { get; set; } = false;
}
