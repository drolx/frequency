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
using Microsoft.Extensions.Logging;
using Proton.Frequency.Device.Handlers.ReaderConnections;
using Proton.Frequency.Device.Helpers;
using Proton.Frequency.Device.Protocols;

namespace Proton.Frequency.Device.Handlers
{
    public class ReaderProcess
    {
        private readonly ConfigKey _config;
        private readonly ILogger<ReaderProcess> _logger;
        private readonly SerialConnection _serial;
        private readonly WebSync _webSync;
        private readonly SerialPort _serialProfile;
        private IReaderProtocol _selectedProtocol;

#if DEBUG
        private bool DevMode = true;
#else
        private bool DevMode = false;
#endif
        public ReaderProcess(
                ILogger<ReaderProcess> logger,
                ConfigKey config,
                SerialConnection serialConnection,
                WebSync webSync,
                IReaderProtocol selectedProtocol
        )
        {
            _config = config;
            _logger = logger;
            _serial = serialConnection;
            _webSync = webSync;
            _serialProfile = _serial.BuildConnection();
            _selectedProtocol = selectedProtocol;
        }

        public async Task SerialConnection()
        {
            // Thread maintenance Timer.
            await Task.Run(() =>
            {
                var sectCheck = new System.Timers.Timer { Interval = 85000, AutoReset = true, Enabled = true };
                sectCheck.Elapsed += OnTimedEvent;

                void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
                {
                    Task.Factory.StartNew(() =>
                        _logger.LogInformation("*****   Reader Thread Maintenance..   *****"));
                }
            });


            // Data received handler method.
            int DataLength = _selectedProtocol.DataLength;
            List<byte> byteList = new List<byte>();

            void DataReceivedHandler(
                object sender,
                SerialDataReceivedEventArgs e)
            {
                SerialPort port = (SerialPort)sender;
                var internalBytes = new byte[port.BytesToRead];

                if (port.IsOpen)
                {
                    try
                    {
                        port.Read(internalBytes, 0, internalBytes.Length);
                        byteList.AddRange(internalBytes);
                        if (byteList.Count <= DataLength)
                        {
                            Task.Factory.StartNew(HandleSerialList);
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
                        _logger.LogError(exception.ToString());
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
                            _logger.LogInformation("Serial connection opened successfully..");
                        }
                        _serialProfile.DiscardInBuffer();
                        _serialProfile.DiscardOutBuffer();

                    }
                    catch (UnauthorizedAccessException unauthorizedAccessException)
                    {
                        var _exp = unauthorizedAccessException;
                        var condLog = _config.IOT_SERIAL_CONN_RETRY == 0 ? "retrying now." : $"retrying now, {maxRetries} remaining.";
                        _logger.LogError($"Serial connection failed to open/read, {condLog}");
                        maxRetries--;
                        retryState = retryFailedCheck;
                        Thread.Sleep(_config.IOT_SERIAL_CONN_TIMEOUT);
                    }
                    catch (Exception e)
                    {
                        var _exp = e;
                        _logger.LogError("Serial connection failed to open/read, retrying now.");
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
                _selectedProtocol.ReceivedData = newRange.ToArray();
                if (_config.IOT_AUTO_READ)
                    Task.Factory.StartNew(() => _selectedProtocol.Log()).Wait();
            }

            // List devices
            _serial.ShowPorts();
            _serialProfile.DataReceived += DataReceivedHandler;

            _logger.LogInformation("Opening new serial connection...");

            // Start Serial connection.
            serialOpen();

            // Development test for hard-coded Hex values.
            if (DevMode && !_serialProfile.IsOpen || !_selectedProtocol.AutoRead)
            {
                var TimerLimit = 1500;
                if (DevMode)
                {
                    _logger.LogDebug("Switching to byte simulation mode...");
                    TimerLimit = 12000;
                }
                var devTest = new System.Timers.Timer { Interval = TimerLimit, AutoReset = true, Enabled = true };
                devTest.Enabled = true;
                devTest.Elapsed += OnDevTest;

                void OnDevTest(Object source, System.Timers.ElapsedEventArgs e)
                {
                    Task.Factory.StartNew(() => _serial.ManuallyReadData(_serialProfile, _selectedProtocol));
                }
            }
            else if (!DevMode && !_serialProfile.IsOpen)
            {
                int maxRetries = _config.IOT_SERIAL_CONN_RETRY;
                while (maxRetries > 1)
                {
                    maxRetries--;
                    serialOpen();
                    await Task.Delay(2000);
                }

                /**
                String processDirectory = System.AppContext.BaseDirectory;
                String processPath = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
                String path = Path.Combine(processDirectory, processPath);
                if (Path.IsPathFullyQualified(path))
                {
                    String escapedArgs = path.Replace("\"", "\\\"");
                    String _filename = null;
                    String _arguments = null;
                    if (System.OperatingSystem.IsWindows())
                    {
                        _filename = "cmd.exe";
                        _arguments = $"/c \"{escapedArgs}\"";
                    }
                    else
                    {
                        _filename = "/bin/bash";
                        _arguments = $"-c \"{escapedArgs}\"";
                    }

                    var process = new Process()
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = _filename,
                            Arguments = _arguments,
                            RedirectStandardOutput = true,
                            UseShellExecute = false,
                            CreateNoWindow = true,
                        }
                    };

                    _logger.LogWarning("Restarting application...");
                    process.Start();
                    process.WaitForExit();
                }
                */

                _logger.LogWarning("Shutting daemon process...");
                /** System.Diagnostics.Process.Start(path); **/
                Environment.Exit(0);

            }
        }

    }
}
