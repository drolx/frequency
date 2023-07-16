using System.ComponentModel.DataAnnotations;
using System.Net;
using Frequency.Common.Interfaces;

namespace Frequency.Common.Config;

public sealed class TerminalConfig : IConfig {
    public const string Key = "terminal";
    public IPAddress Host { get; set; } = IPAddress.Any;

    [Range(0, 65535, ErrorMessage = "Port number invalid.")]
    public int Port { get; set; } = 8080;
    public bool Auto { get; set; } = false;
    public int Retries { get; set; } = 3;
    public int Timeout { get; set; } = 5;
}