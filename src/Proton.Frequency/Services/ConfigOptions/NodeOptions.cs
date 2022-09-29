using System.ComponentModel.DataAnnotations;

namespace Proton.Frequency.Services.ConfigOptions;

public class NodeOptions
{
    public const string SectionKey = "Node";

    public string Host { get; set; } = "localhost";

    [Range(0, 65535, ErrorMessage = "Port number invalid.")]
    public int Port { get; set; } = 8080;

    public bool Auto { get; set; } = false;

    public int Retries { get; set; } = 3;

    public int Timeout { get; set; } = 5;
}
