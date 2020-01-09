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
using System;
using System.IO.Ports;
using System.Threading;
using uhf_rfid_catch.Helpers;

namespace uhf_rfid_catch.Handlers.ReaderConnections
{
    public class SerialConnection
    {
        private static readonly ConfigContext SettingsContext = new ConfigContext();
        MainLogger _logger = new MainLogger();
        private static readonly string SPORTNAME = SettingsContext.Resolve("ReaderSerialPortName");
        private static readonly int SBAUDRATE = Convert.ToInt32(SettingsContext.Resolve("ReaderSerialBaudRate"));
        private static readonly int SDATABITS = Convert.ToInt32(SettingsContext.Resolve("ReaderSerialDataBits"));
        private static readonly int SMAXRETRY = Convert.ToInt32(SettingsContext.Resolve("ReaderConnectionRetries"));

        public SerialConnection()
        {
        }

        private SerialPort BuildConnection(string knownPortName)
        {
            var portName = knownPortName == "null" ? SuggestPort() : knownPortName;

            var parity = Parity.None;
            var stopBits = StopBits.One;
            var srp = new SerialPort(portName, SBAUDRATE, parity, SDATABITS, stopBits);
            return srp;
        }

        private string SuggestPort()
        {
            var selectedPort = "";

            foreach (string portName in ListConnection())
            {
                if(portName.Contains("serial") || portName.Contains("uart"))
                {
                    selectedPort = portName;
                    break;
                }
            }

            return selectedPort;
        }

        public byte ConnectionChannel()
        {
            int maxRetries = SMAXRETRY;
            const int sleepTimeInMs = 2000;
            byte responseData;
            
            if (!BuildConnection(SPORTNAME).IsOpen)
            {
                _logger.Trigger("Error", $"Serial connection failed, retrying now.");
                try
                {
                    while (maxRetries > 0)
                    {
                        var recoverConn = BuildConnection(SuggestPort());
                        recoverConn.Open();
                        if (recoverConn.BytesToRead > 0)
                        {
                            responseData = (byte)recoverConn.ReadByte();
                            return responseData;
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.Trigger("Error", $"Serial connection failed again, {maxRetries} retry remaining.");
                    maxRetries--;
                    Console.WriteLine(e);
                    Thread.Sleep(sleepTimeInMs);
                }
            }
                var workingConn = BuildConnection(SPORTNAME);
                workingConn.Open();
                
//                if (workingConn.BytesToRead > 0)

                responseData = (byte)workingConn.ReadByte();
                return responseData;
        }

        private string[] ListConnection()
        {
            return SerialPort.GetPortNames();
        }
        public void ShowPorts()
        {
            string startLine = $"---- {ListConnection().Length} Serial ports available ----";
            Console.WriteLine(startLine);
            foreach (string portName in ListConnection())
            {
                Console.WriteLine(portName);
            }
        }

//        private byte SendData(SerialPort serialConnection)
//        {
//            if (SerialConnection.BytesToRead > 0)
//            {
//                
//            }
//        }
        
        
    }
}
