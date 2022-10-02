using System.ComponentModel.DataAnnotations;

namespace Proton.Frequency.Config;

public sealed class QueueConfig : IConfig
{
    public static string Key { get; set; } = "server:queue";
    private static bool List { get; set; } = false;
    public string Identifier => Key;
    public bool IsList => List;
    public bool Enable { get; set; }

    [Range(minimum: 0, maximum: 65535, ErrorMessage = "Port number invalid.")]
    public int Port { get; set; } = 1883;
}
