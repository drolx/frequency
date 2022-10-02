using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Proton.Frequency.Config;

public sealed class ServerConfig : IConfig
{
    public static string Key { get; set; } = "server";
    private static bool List { get; set; } = false;
    public string Identifier => Key;
    public bool IsList => List;
    public IPAddress Host { get; set; } = IPAddress.Any;

    [Range(minimum: 0, maximum: 65535, ErrorMessage = "Port number invalid.")]
    public int Port { get; set; } = 8080;
}
