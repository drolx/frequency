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
using System.Threading.Tasks;
using uhf_rfid_catch.Helpers;
using uhf_rfid_catch.Protocols;

namespace uhf_rfid_catch.Handlers.ReaderConnections
{
    public class SerialConnection
    {
        private readonly ConfigKey _config;
        private readonly MainLogger _logger;
        private readonly ByteAssist _assist;
//        private readonly ConsoleOnlyLogger _consoleOnlyLogger = new ConsoleOnlyLogger();

        public SerialConnection()
        {
            _config = new ConfigKey();
            _logger = new MainLogger();
            _assist = new ByteAssist();
        }

        public SerialPort BuildConnection()
        {
            var portName = _config.IOT_SERIAL_PORTNAME == "null" ? SuggestPort() : _config.IOT_SERIAL_PORTNAME;

            var parity = Parity.None;
            var stopBits = StopBits.One;
            var srp = new SerialPort(portName, _config.IOT_SERIAL_BAUDRATE, parity, _config.IOT_SERIAL_DATABITS, stopBits)
            {
                DtrEnable = true, RtsEnable = true, ReadTimeout = 500, WriteTimeout = 500, Handshake = Handshake.None, DiscardNull = true
            };
            return srp;
        }

        public string SuggestPort()
        {
            var selectedPort = "/dev/tty.usb_serial";

            foreach (string portName in ListConnection())
            {
                if(portName.Contains("serial")
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

        public byte ConnectionChannel(SerialPort builtConnection)
        {
            var receivedByte = (byte)builtConnection.ReadByte();
            return receivedByte;
        }

        public void AutoReadData(SerialPort builtConnection, IReaderProtocol protoInfo)
        {
            var localByteSize = 0;
            int localMaxSize = 0;
            byte[] decodedBytes = new byte[protoInfo.DataLength];
            while (_config.IOT_AUTO_READ)
            {
#if DEBUG
                if (true)
                {
#endif
#if !DEBUG
                    if (builtConnection.IsOpen)
                    {
#endif
                    if(protoInfo.AutoRead)
                    {
                        // Start decode part of the process.
                        ////
                        if (builtConnection.IsOpen && builtConnection.BytesToRead > 0)
                        {
                            var _returnedData = ConnectionChannel(builtConnection);
                            try
                            {
                                decodedBytes[localByteSize] = _returnedData;
                            }
                            catch (Exception e)
                            {
                                _logger.Trigger("Error", e.ToString());
                            }
                            
                            if (localMaxSize - 1 == localByteSize)
                            {
                                if (builtConnection.IsOpen)
                                {
                                    protoInfo.ReceivedData = decodedBytes;
                                    var persistScan = new Task(protoInfo.Log);
                                    persistScan.Start();
                                }
                            }
                            
                            ++localByteSize;
                        }
                        else
                        {
                            if (localByteSize != 0 && localByteSize > 2 )
                            {
                                localMaxSize = localByteSize > localMaxSize ? localByteSize : localMaxSize;
                                if (localMaxSize == localByteSize)
                                {
                                    decodedBytes = new byte[protoInfo.DataLength];
                                }
                                
                            }
                            
                            localByteSize = 0;
                        }
                        
#if DEBUG
                        if (!builtConnection.IsOpen)
                        {
                            protoInfo.ReceivedData =
                                _assist.HexToByteArray("CCFFFF10320D01E2000016370402410910C2E9AC");
                            
                            // Logging and persisting task
                            var persistScan = new Task(protoInfo.Log);
                            persistScan.Start();
                            
                            Thread.Sleep(10000);
                        }
#endif
                    }
                    else
                    {
                        Console.WriteLine("Implement non auto read mode.");
                        // TODO: Manual serial port Read with a command.
                        // Sample of reading every 5 seconds
                        // After Executing write commands and getting response.
                        
                        // Then sleep for a while.
                        Thread.Sleep(5000);
                    }
                    
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
        
        public void DataReceivedHandler(
            object sender,
            SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort) sender;
            byte[] buf = new byte[sp.BytesToRead];
            sp.Read(buf, 0, buf.Length);
            Console.WriteLine(BitConverter.ToString(buf));
        }
        
    }
}
