//
// ReaderConnection.cs
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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using uhf_rfid_catch.Handlers.ReaderConnections;
using uhf_rfid_catch.Helpers;
using uhf_rfid_catch.Protocols;

namespace uhf_rfid_catch.Handlers
{
    public class ReaderConnection
    {
        private readonly ConfigKey _config;
        private readonly MainLogger _logger;
        private readonly SerialConnection _serial;
        private readonly WebSync _webSync;
        
#if DEBUG
        private bool DevMode = true;
#else
        private bool DevMode = false;
#endif
        public ReaderConnection()
        {
            _config = new ConfigKey();
            _logger = new MainLogger();
            _serial = new SerialConnection();
            _webSync = new WebSync();
        }

        public void SerialConnection()
        {
            SerialPort serialProfile = _serial.BuildConnection();
            IReaderProtocol _selectedProtocol = new BaseReaderProtocol().Resolve();
            
            int DataLength = _selectedProtocol.DataLength;
            List<byte> byteList = new List<byte>();
        
            void DataReceivedHandler(
                object sender,
                SerialDataReceivedEventArgs e)
            {
                SerialPort port = (SerialPort)sender;
                var internalBytes = new byte[port.BytesToRead];
                port.Read(internalBytes, 0, internalBytes.Length);
                byteList.AddRange(internalBytes);
                if (byteList.Count <= DataLength)
                {
                    var tsk = new Task(HandleserialList);
                    tsk.Start();
                }
                else
                {
                    if (byteList.Count > DataLength + 3)
                    {
                        byteList.Clear();
                    }
                    else
                    {
                        byteList.RemoveRange(0, byteList.Count - DataLength);
                    }
                }
                
            }

            void HandleserialList()
            {
                var newRange = byteList.GetRange(0, DataLength);
                byteList.RemoveRange(0, DataLength);
                // Start log process.
                _selectedProtocol.ReceivedData = newRange.ToArray();
                Task.Factory.StartNew(_selectedProtocol.Log);
            }
            
            // List devices
            _serial.ShowPorts();
            serialProfile.DataReceived += DataReceivedHandler;

            _logger.Trigger("Info", $"Opening new serial connection...");
            
            var maxRetries = _config.IOT_SERIAL_CONN_RETRY;
            var retryState = true;
            var retryFailedCheck = true;
            // Failure handler start process
            while (retryState)
            {
                try
                {
                    if (maxRetries > 0 || _config.IOT_SERIAL_CONN_RETRY == 0)
                    {
                        retryState = false;
                        retryFailedCheck = true;
                    }
                    else
                    {
                        retryFailedCheck = false;
                    }

                    if (serialProfile.IsOpen && serialProfile != null)
                    {
                        serialProfile.Close();
                        serialProfile.Dispose();
                    }
                    serialProfile.Open();

                    if (serialProfile.IsOpen)
                    {
//                        Console.ReadKey();
                        _logger.Trigger("Info", $"Serial connection opened successfully...");
                    }
                    
                    // Clear buffer to avoid out-of-bounds exceptions
                    serialProfile.DiscardInBuffer();
                    serialProfile.DiscardOutBuffer();
                    
                }
                catch (UnauthorizedAccessException unauthorizedAccessException)
                {
                    var _exp = unauthorizedAccessException;
                    var condLog = _config.IOT_SERIAL_CONN_RETRY == 0 ? "retrying now." : $"retrying now, {maxRetries} remaining.";
                    _logger.Trigger("Error", $"Serial connection failed to open/read, {condLog}");
                    maxRetries--;
                    retryState = retryFailedCheck;
                    Thread.Sleep(_config.IOT_SERIAL_CONN_TIMEOUT);
                }
                catch (Exception e)
                {
                    var _exp = e;
                    _logger.Trigger("Error", "Serial connection failed to open/read, retrying now.");
                    maxRetries--;
                    retryState = retryFailedCheck;
                    Thread.Sleep(_config.IOT_SERIAL_CONN_TIMEOUT);
                }

                if (DevMode || serialProfile.IsOpen)
                {
                    // Timed Thread start for Timer based scanning.
                    
                    if (DevMode)
                    {
                        // Development test for hard-coded Hex values.
                        var devTest = new System.Timers.Timer {Interval = 10000, AutoReset = true, Enabled = true};
                        devTest.Elapsed += OnDevTest;
                    
                        void OnDevTest(Object source, System.Timers.ElapsedEventArgs e) {
//                            Task.Factory.StartNew(() => _serial.AutoReadData(serialProfile, _selectedProtocol));
                        }
                    }

                    // Thread maintenance Timer.
                    var sectCheck = new System.Timers.Timer {Interval = 15000, AutoReset = true, Enabled = true};
                    sectCheck.Elapsed += OnTimedEvent;
                    
                    void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e) {
                        Task.Factory.StartNew(() => _logger.Trigger("Info", "Keep Thread Alive.."));
                    }

                }
            }
            // Failure handler end process
        }

        public void NetworkConnection()
        {

        }

        // Main Reader method for bootstrapping everything from the main thread.
        public void Run()
        {
            SerialConnection();
            
            // Cloud sync timed thread sub process.
            var webSyncTimer = new System.Timers.Timer {Interval = 10000, AutoReset = true, Enabled = true};
            webSyncTimer.Elapsed += OnWebSyncEvent;
                    
            void OnWebSyncEvent(Object source, System.Timers.ElapsedEventArgs e) {
                var syncTask = new Task(_webSync.Sync);
                syncTask.Start();
            }
        }
        
    }
}
