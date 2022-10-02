using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Proton.Frequency.Config;

public sealed class ServerConfig
{
    public const string Key = "Server";
    public IPAddress Host { get; set; } = IPAddress.Any;
    [Range(0, 65535, ErrorMessage = "Port number invalid.")]
    public int Port { get; set; } = 8080;
}
