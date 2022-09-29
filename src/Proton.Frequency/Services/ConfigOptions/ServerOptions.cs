using System.ComponentModel.DataAnnotations;

namespace Proton.Frequency.Services.ConfigOptions;

internal class ServerOptions
{
    public const string SectionKey = "Server";

    public string Host { get; set; } = "localhost";

    [Range(0, 65535, ErrorMessage = "Port number invalid.")]
    public int Port { get; set; } = 8080;
}
