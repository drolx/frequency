using Frequency.Common.Interfaces;

namespace Frequency.Common.Config;

public class NetConfig : IConfig {
    public const string Key = "net-client";
    public string Id { get; set; } = "net-00";
    public string Type { get; set; } = "chafon";
    public int Port { get; set; } = 1730;
}