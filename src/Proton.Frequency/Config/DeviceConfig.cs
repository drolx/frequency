using System.IO.Ports;

namespace Proton.Frequency.Config; 

public sealed class DeviceConfig {
    public const string Key = "devices";
    public string Name { get; set; } = "node-00";
    public string Port { get; set; } = "/dev/ttyUSB0";
    public int Rate { get; set; } = 9600;
    public int Bit { get; set; } = 8;
    public Parity Parity { get; set; } = Parity.None;
    public StopBits StopBit { get; set; } = StopBits.One;
    public Handshake HandShake { get; set; } = Handshake.None;
}
