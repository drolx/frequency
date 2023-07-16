using System.ComponentModel.DataAnnotations;
using System.Net;
using Proton.Frequency.Common.Interfaces;

namespace Proton.Frequency.Common.Config;

public sealed class ServerConfig : IConfig {
    public const string Key = "server";
    public string Name { get; set; } = "RFID Service";
    public IPAddress Host { get; set; } = IPAddress.Any;

    [Range(0, 65535, ErrorMessage = "Port number invalid.")]
    public int Port { get; set; } = 8080;

    public bool Resources { get; set; } = false;
}