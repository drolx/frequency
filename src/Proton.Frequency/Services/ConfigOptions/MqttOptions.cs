using System.ComponentModel.DataAnnotations;

namespace Proton.Frequency.Services.ConfigOptions;

public class MqttOptions
{
    public const string SectionKey = "Server:Queue";

    public bool Enable { get; set; } = true;

    [Range(0, 65535, ErrorMessage = "Port number invalid.")]
    public int Port { get; set; } = 1883;
}
