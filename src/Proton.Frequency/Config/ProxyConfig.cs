using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Proton.Frequency.Config;

public sealed class ProxyConfig
{
    public const string Key = "proxy";
    public IPAddress Host { get; set; } = IPAddress.Any;
    [Range(0, 65535, ErrorMessage = "Port number invalid.")]
    public int Port { get; set; } = 8080;
    public bool Auto { get; set; } = false;
    public int Retries { get; set; } = 3;
    public int Timeout { get; set; } = 5;
}
