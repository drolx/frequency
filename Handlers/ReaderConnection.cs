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
        private readonly ConsoleLogger _consolelog;
        private readonly SerialConnection _serial;
        private readonly WebSync _webSync;
        private readonly SerialPort _serialProfile;
        
#if DEBUG
        private bool DevMode = true;
#else
        private bool DevMode = false;
#endif
        public ReaderConnection()
        {
            _config = new ConfigKey();
            _logger = new MainLogger();
            _consolelog = new ConsoleLogger();
            _serial = new SerialConnection();
            _webSync = new WebSync();
            _serialProfile = _serial.BuildConnection();
        }

        public void SerialConnection()
        {
            IReaderProtocol _selectedProtocol = new BaseReaderProtocol().Resolve();

            // Thread maintenance Timer.
            var sectCheck = new System.Timers.Timer {Interval = 35000, AutoReset = true, Enabled = true};
            sectCheck.Elapsed += OnTimedEvent;
            void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e) {
                Task.Factory.StartNew(() => _consolelog.Trigger("Info", "*****   Keep Thread Alive   *****"));
            }
            
            // Data received handler method.
            int DataLength = _selectedProtocol.DataLength;
            List<byte> byteList = new List<byte>();
        
            void DataReceivedHandler(
                object sender,
                SerialDataReceivedEventArgs e)
            {
                SerialPort port = (SerialPort)sender;
                var internalBytes = new byte[port.BytesToRead];

                if(port.IsOpen)
                {
                    try
                    {
                        port.Read(internalBytes, 0, internalBytes.Length);
                        byteList.AddRange(internalBytes);
                        if (byteList.Count <= DataLength)
                        {
                            var tsk = new Task(HandleSerialList);
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
                    catch (Exception exception)
                    {
                        _logger.Trigger("Error", exception.ToString());
                    }
                }
                
            }

            // Connection state handler.
            void serialOpen()
            {
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

                        if (_serialProfile.IsOpen && _serialProfile != null)
                        {
                            _serialProfile.Close();
                            _serialProfile.Dispose();
                        }
                        _serialProfile.Open();

                        if (_serialProfile.IsOpen)
                        {
                            _logger.Trigger("Info", "Serial connection opened successfully..");
                        }

                        // Clear buffer to avoid out-of-bounds exceptions
                        _serialProfile.DiscardInBuffer();
                        _serialProfile.DiscardOutBuffer();

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

                }
            }

            void HandleSerialList()
            {
                var newRange = byteList.GetRange(0, DataLength);
                byteList.RemoveRange(0, DataLength);
                // Start log process.
                _selectedProtocol.ReceivedData = newRange.ToArray();
                Task.Factory.StartNew(() => _selectedProtocol.Log());
            }
            
            // List devices
            _serial.ShowPorts();
            _serialProfile.DataReceived += DataReceivedHandler;

            _logger.Trigger("Info", "Opening new serial connection...");

            // Start Serial connection.
            serialOpen();
            
            // Development test for hard-coded Hex values.
            if (DevMode && !_serialProfile.IsOpen || !_selectedProtocol.AutoRead)
            {
                var TimerLimit = 1500;
                if (DevMode)
                {
                    _logger.Trigger("Debug", "Switching byte simulation mode..");
                    TimerLimit = 12000;
                }
                var devTest = new System.Timers.Timer {Interval = TimerLimit, AutoReset = true, Enabled = true};
                devTest.Enabled = true;
                devTest.Elapsed += OnDevTest;
                
                void OnDevTest(Object source, System.Timers.ElapsedEventArgs e)
                {
                    Task.Factory.StartNew(() => _serial.ManuallyReadData(_serialProfile, _selectedProtocol));
                }
            }
        }

        public void NetworkConnection()
        {

        }

        // Main Reader method for bootstrapping everything from the main thread.
        public void Run()
        {
            SerialConnection();
            
            // Cloud sync timed thread sub process.
            if (_config.IOT_MODE_ENABLE && _config.IOT_REMOTE_HOST_ENABLE)
            {
                var webSyncTimer = new System.Timers.Timer {Interval = _config.IOT_MIN_REMOTE_FREQ, AutoReset = true, Enabled = true};
                webSyncTimer.Elapsed += OnWebSyncEvent;
                    
                void OnWebSyncEvent(Object source, System.Timers.ElapsedEventArgs e) {
                    var syncTask = new Task(_webSync.Sync);
                    syncTask.Start();
                }
            }
            
        }
        
    }
}
