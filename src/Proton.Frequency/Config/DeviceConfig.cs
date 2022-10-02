using System.IO.Ports;

namespace Proton.Frequency.Config;

public sealed class DeviceConfig : IConfig
{
    public static string Key { get; set; } = "devices";
    private static bool List { get; set; } = true;
    public string Identifier => Key;
    public bool IsList => List;
    public string Name { get; set; } = "node-00";
    public string Port { get; set; } = "/dev/ttyUSB0";
    public int Rate { get; set; } = 9600;
    public int Bit { get; set; } = 8;
    public Parity Parity { get; set; } = Parity.None;
    public StopBits StopBit { get; set; } = StopBits.One;
    public Handshake HandShake { get; set; } = Handshake.None;
}
