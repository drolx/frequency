using Proton.Frequency.Common.Interfaces;

namespace Proton.Frequency.Common.Config;

public sealed class ServiceConfig : IConfig {
    public const string Key = "service";
    public bool Proxy { get; set; } = false;
    public bool Management { get; set; } = true;
    public bool Api { get; set; } = false;
    public bool Network { get; set; } = false;
    public string Secret { get; set; } = "secret";
    public bool Event { get; set; } = true;
}
