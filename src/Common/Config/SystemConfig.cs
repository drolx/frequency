using System.ComponentModel.DataAnnotations;
using Frequency.Common.Interfaces;

namespace Frequency.Common.Config;

public sealed class SystemConfig : IConfig {
    public const string Key = "system";
    public string Name { get; set; } = "RFID Service";
    public bool Terminal { get; set; } = false;
    public bool Web { get; set; } = true;
    public bool Api { get; set; } = false;
    [Range(0, 65535, ErrorMessage = "Port number invalid.")]
    public int Port { get; set; } = 7001;
    public bool Network { get; set; } = false;
    public string Secret { get; set; } = "secret";
    public bool Event { get; set; } = true;
}