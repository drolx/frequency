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
using uhf_rfid_catch.Protocols;

namespace uhf_rfid_catch.Handlers.ReaderConnections
{
    public class SerialConnection
    {
        private static readonly ConfigContext SettingsContext = new ConfigContext();
        MainLogger _logger = new MainLogger();
        public readonly string Sportname = SettingsContext.Resolve("ReaderSerialPortName");
        public readonly int Sbaudrate = Convert.ToInt32(SettingsContext.Resolve("ReaderSerialBaudRate"));
        public readonly int Sdatabits = Convert.ToInt32(SettingsContext.Resolve("ReaderSerialDataBits"));
        public readonly int Smaxretry = Convert.ToInt32(SettingsContext.Resolve("ReaderConnectionRetries"));
        public readonly int Smaxtimeout = Convert.ToInt32(SettingsContext.Resolve("ReaderConnectionTimeout"));
        public bool AutoRead = Convert.ToBoolean(SettingsContext.Resolve("ReaderAutoReadMode"));

        public SerialConnection()
        {
        }

        public SerialPort BuildConnection(string knownPortName)
        {
            var portName = knownPortName == "null" ? SuggestPort() : knownPortName;

            var parity = Parity.None;
            var stopBits = StopBits.One;
            var srp = new SerialPort(portName, Sbaudrate, parity, Sdatabits, stopBits)
            {
                DtrEnable = true, RtsEnable = true
            };
            return srp;
        }

        public string SuggestPort()
        {
            var selectedPort = "/dev/tty.usb_serial";

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

        public byte ConnectionChannel(SerialPort builtConnection)
        {
            var receivedByte = (byte)builtConnection.ReadByte();
            return receivedByte;
        }

        public void AutoReadData(SerialPort builtConnection, IReaderProtocol protoInfo)
        {
            var localByteSize = 0;
            var localMaxByteSize = protoInfo.AutoReadLength;
            byte[] decodedBytes = new byte[localMaxByteSize];
            while (AutoRead)
            {
                if (builtConnection.IsOpen)
                {
                    if(protoInfo.DirectAutoRead)
                    {
                        // Start decode part of the process.
                        ////
                        if (builtConnection.BytesToRead > 0)
                        {
                            var _returnedData = ConnectionChannel(builtConnection);
                            try
                            {
                                decodedBytes[localByteSize] = _returnedData;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                                throw;
                            }
                            
                            if (localMaxByteSize - 1 == localByteSize)
                            {
                                Console.WriteLine("Lst ray pos ->" + localByteSize);
                                Console.WriteLine(BitConverter.ToString(decodedBytes).Replace("-", string.Empty));
                            }
                            ++localByteSize;
                        }
                        else
                        {
                            localByteSize = 0;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Implement non auto read mode.");
                    }
                    
                }
                else
                {
////                    RequestStop();
                    _logger.Trigger("Error", $"Serial connection failed to open, retrying now.");
                }
                
            }
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
    }
}
