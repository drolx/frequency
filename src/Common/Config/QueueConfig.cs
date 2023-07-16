using System.ComponentModel.DataAnnotations;
using Proton.Frequency.Common.Interfaces;

namespace Proton.Frequency.Common.Config;

public sealed class QueueConfig : IConfig {
    public const string Key = "system:queue";
    public bool Enable { get; set; }
    
    [Range(0, 65535, ErrorMessage = "Port number invalid.")]
    public int Port { get; set; } = 1883;
}
