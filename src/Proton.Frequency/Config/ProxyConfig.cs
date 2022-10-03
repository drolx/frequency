using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Proton.Frequency.Config;

public sealed class ProxyConfig : IConfig {
    public const string Key = "proxy";
    public const bool IsList = false;
    public IPAddress Host { get; set; } = IPAddress.Any;

    [Range(minimum: 0, maximum: 65535, ErrorMessage = "Port number invalid.")]
    public int Port { get; set; } = 8080;

    public bool Auto { get; set; } = false;
    public int Retries { get; set; } = 3;
    public int Timeout { get; set; } = 5;
}
