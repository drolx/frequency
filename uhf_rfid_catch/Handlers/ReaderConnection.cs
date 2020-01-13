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
using uhf_rfid_catch.Handlers.ReaderConnections;
using uhf_rfid_catch.Helpers;
namespace uhf_rfid_catch.Handlers
{
    public class ReaderConnection
    {
        private static readonly ConfigContext SettingsContext = new ConfigContext();
        MainLogger _logger = new MainLogger();
        private byte[] _decodedSerialBytes;
        private int _serialBytesLength = 0;
        
        private string HexProps { get; set; }

        public ReaderConnection()
        {
        }

        public void SerialConnection()
        {
            bool shouldStop = false;
            SerialConnection spry = new SerialConnection();
            var serialProfile = spry.BuildConnection(spry.Sportname);
            // List devices
            spry.ShowPorts();
//            SerialProfile.DataReceived += spry.portOnReceiveData;
            var maxRetries = 25;
            try
            {
                if (maxRetries > 0)
                {
                    serialProfile.Open();

                    // Thread start for Auto scanning readers
                    var autoScanThread = new Thread(() => spry.AutoReadData(serialProfile));
                    autoScanThread.Start();
                    
                    Console.WriteLine("Test For other modes");
                    
                }
            }
            catch (Exception e)
            {
                _logger.Trigger("Error", $"Serial connection failed to open/read, retrying now.");
                maxRetries--;
                Thread.Sleep(500);
            }
            
            void RequestStop()
            {
                serialProfile.DtrEnable = true;
                serialProfile.RtsEnable = true;
                serialProfile.Open();
//                _shouldStop = true;
            }
            
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
