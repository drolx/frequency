using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Proton.Frequency.Config;

public sealed class ServerConfig : IConfig
{
    public const string Key = "server";
    public const bool IsList = false;
    public string Name { get; set; } = "RFID Service";
    public IPAddress Host { get; set; } = IPAddress.Any;

    [Range(minimum: 0, maximum: 65535, ErrorMessage = "Port number invalid.")]
    public int Port { get; set; } = 8080;
    public bool Resources { get; set; } = false;
}
