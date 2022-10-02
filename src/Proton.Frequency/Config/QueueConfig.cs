using System.ComponentModel.DataAnnotations;

namespace Proton.Frequency.Config;

public sealed class QueueConfig
{
    public const string Key = "server:queue";
    public bool Enable { get; set; }
    [Range(0, 65535, ErrorMessage = "Port number invalid.")]
    public int Port { get; set; } = 1883;
}
