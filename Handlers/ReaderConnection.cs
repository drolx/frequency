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
using System.IO.Ports;
using System.Threading;
using uhf_rfid_catch.Handlers.ReaderConnections;
using uhf_rfid_catch.Helpers;
using uhf_rfid_catch.Protocols;

namespace uhf_rfid_catch.Handlers
{
    public class ReaderConnection
    {
        private static readonly ConfigKey _config = new ConfigKey();
        MainLogger _logger = new MainLogger();

        public ReaderConnection()
        {
        }

        public void SerialConnection()
        {
            SerialConnection spry = new SerialConnection();
            SerialPort serialProfile = spry.BuildConnection();
            BaseReaderProtocol selectedProtocol = new BaseReaderProtocol();
            IReaderProtocol _selectedProtocol = selectedProtocol.Resolve();

            // List devices
            spry.ShowPorts();
//            serialProfile.DataReceived += spry.DataReceivedHandler;
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
                    
                    _logger.Trigger("Info", $"Opening new serial connection...");
                    
                    serialProfile.Open();
                    
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
#if DEBUG
                    if (serialProfile.IsOpen || maxRetries == 0 || _config.IOT_SERIAL_CONN_RETRY == 0)
                    {
#endif
#if !DEBUG
                    if (serialProfile.IsOpen)
                    {
#endif
                    ///////
                    // Thread start for Auto scanning readers
                    var autoScanThread = new Thread(() => spry.AutoReadData(serialProfile, _selectedProtocol));
                    autoScanThread.Name = "Reader Auto Scanner";
                    autoScanThread.Start();

                    ///////
                    // Add other modes
                    Console.WriteLine("Test For other modes..");
                    
                }
            }
            // Failure handler end process
        }

        public void NetworkConnection()
        {

        }

        public void Run()
        {
            SerialConnection();

        }
        
    }
}
