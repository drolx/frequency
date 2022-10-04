using System.ComponentModel.DataAnnotations;

namespace Proton.Frequency.Config;

public sealed class QueueConfig : IConfig
{
    public const string Key = "server:queue";
    public const bool IsList = false;
    public bool Enable { get; set; }

    [Range(minimum: 0, maximum: 65535, ErrorMessage = "Port number invalid.")]
    public int Port { get; set; } = 1883;
}
