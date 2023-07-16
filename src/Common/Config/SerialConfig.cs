using Proton.Frequency.Common.Interfaces;

namespace Proton.Frequency.Common.Config;

public class SerialConfig : IConfig {
    public const string Key = "serial";
    public string Id { get; set; } = "serial-00";
    public string Port { get; set; } = "/dev/ttyUSB0";
    public int Rate { get; set; } = 9600;
    public int Bit { get; set; } = 8;
    
    // ReSharper disable once InvalidXmlDocComment
    /** Defaults for serial port
    public Parity Parity { get; set; } = Parity.None;
    public StopBits StopBit { get; set; } = StopBits.One;
    public Handshake HandShake { get; set; } = Handshake.None;
    */
}
