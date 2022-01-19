//
// SerialConnection.cs
//
// Author:
//       Godwin peter .O <me@godwin.dev>
//
// Copyright (c) 2020 MIT
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using Proton.Frequency.Service.Helpers;
using Proton.Frequency.Service.Protocols;

namespace Proton.Frequency.Service.Handlers.ReaderConnections
{
    public class SerialConnection
    {
        private readonly ConfigKey _config;
        private readonly MainLogger _logger;
        private readonly ByteAssist _assist;
        private readonly ConsoleLogger _consolelog;
#if DEBUG
        private bool DevMode = true;
#else
        private bool DevMode = false;
#endif

        public SerialConnection()
        {
            _config = new ConfigKey();
            _logger = new MainLogger();
            _assist = new ByteAssist();
            _consolelog = new ConsoleLogger();
        }

        public SerialPort BuildConnection()
        {
            var portName = _config.IOT_SERIAL_PORTNAME == "null" ? SuggestPort() : _config.IOT_SERIAL_PORTNAME;

            var parity = Parity.None;
            var stopBits = StopBits.One;
            var srp = new SerialPort(portName, _config.IOT_SERIAL_BAUDRATE, parity, _config.IOT_SERIAL_DATABITS, stopBits)
            {
                DtrEnable = true,
                RtsEnable = true,
                ReadTimeout = 500,
                WriteTimeout = 500,
                Handshake = Handshake.None,
                DiscardNull = true
            };
            return srp;
        }

        public string SuggestPort()
        {
            var selectedPort = "/dev/tty.usb_serial";
            var ListConnections = ListConnection();
            _logger.Trigger("Warn", "Incorrect port specified, Suggesting port...");
            foreach (string portName in ListConnections)
            {
                if (portName.Contains("serial")
                   || portName.Contains("uart")
                   || portName.Contains("ttyUSB")
                   || portName.Contains("COM"))
                {
                    selectedPort = portName;
                    break;
                }
            }

            return selectedPort;
        }

        public void ManuallyReadData(SerialPort builtConnection, IReaderProtocol protoInfo)
        {
            if (DevMode && !builtConnection.IsOpen && protoInfo.AutoRead)
            {
                // Start decode part of the process.
                protoInfo.ReceivedData =
                    _assist.HexToByteArray("CCFFFF10320D01E2000016370402410910C2E9AC");

                // Logging and persisting task
                Task.Factory.StartNew(protoInfo.Log);
            }
            else if ((!protoInfo.AutoRead || !_config.IOT_AUTO_READ) && builtConnection.IsOpen)
            {
                Console.WriteLine("Implement non auto read mode.");
                // TODO: Manual serial port Read with a command.
                // Sample of reading every 5 seconds
                // After Executing write commands and getting response.
            }
        }

        private string[] ListConnection()
        {
            return SerialPort.GetPortNames();
        }
        public void ShowPorts()
        {
            var ListConnections = ListConnection();
            string startLine = $"---- {ListConnections.Length} Serial ports available ----";
            _logger.Trigger("Info", startLine);
            foreach (string portName in ListConnections)
            {
                _logger.Trigger("Info", portName);
            }
        }

    }
}
