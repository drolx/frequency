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
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Proton.Frequency.Device.Handlers.ReaderConnections;
using Proton.Frequency.Device.Helpers;
using Proton.Frequency.Device.Protocols;

#nullable enable
namespace Proton.Frequency.Device.Handlers
{
    public class ReaderProcess
    {
        private readonly ConfigKey _config;
        private readonly ILogger<ReaderProcess> _logger;
        private readonly SerialConnection _serial;
        private readonly WebSync _webSync;
        private SerialPort _serialProfile;
        private IReaderProtocol _selectedProtocol;
        private int _protocolDataLength;
        private int _serialBlockLimit;
        private int _serialMaxRetries;

#if DEBUG
        private bool isDevelopment = true;
#else
        private bool isDevelopment = false;
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
            _protocolDataLength = _selectedProtocol.DataLength;
            _serialBlockLimit = 2048;
            _serialMaxRetries = _config.IOT_SERIAL_CONN_RETRY - 1;
        }

        private void handleAppSerialError(Exception exception)
        {
            _logger.LogError("Serial Error");
            _logger.LogDebug($"Error: {exception}");
        }

        private async void raiseAppSerialDataEvent(byte[] received)
        {
            if (received.Length == _protocolDataLength)
            {
                try
                {
                    _selectedProtocol.ReceivedData = received;
                    await Task.Factory.StartNew(() => _selectedProtocol.Log());
                }
                catch (Exception exception)
                {
                    _logger.LogError("Single scan process failed to complete..");
                    _logger.LogDebug($"Error: {exception}");
                }
            }
            else
            {
                int dataLength = received.Length;
                float dataCount = (float)dataLength / (float)_protocolDataLength;
                byte[] chunkBuffer;
                if (received.Length % _protocolDataLength == 0)
                {
                    try
                    {
                        Task.Factory.StartNew(async () =>
                            {
                                for (int i = 0; i < received.Length; i += _protocolDataLength)
                                {
                                    chunkBuffer = new byte[_protocolDataLength];
                                    Array.Copy(received, i, chunkBuffer, 0, _protocolDataLength);
                                    _selectedProtocol.ReceivedData = chunkBuffer;
                                    await _selectedProtocol.Log();
                                    await Task.Delay(20);
                                }
                            }).Wait();
                    }
                    catch (Exception exception)
                    {
                        _logger.LogError("Large pool process failed to complete..");
                        _logger.LogDebug($"Error: {exception}");
                    }
                }
            }

        }

        public async Task StartChannel(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Serial port connection complete..");
            _serialProfile.DiscardInBuffer();
            _serialProfile.DiscardOutBuffer();
            byte[] buffer = new byte[_serialBlockLimit];
            Action? kickoffRead = null;
            kickoffRead = delegate
            {
                _serialProfile.BaseStream.BeginRead(buffer, 0, buffer.Length, delegate (IAsyncResult ar)
                {
                    try
                    {
                        int actualLength = _serialProfile.BaseStream.EndRead(ar);
                        byte[] received = new byte[actualLength];
                        Buffer.BlockCopy(buffer, 0, received, 0, actualLength);
                        raiseAppSerialDataEvent(received);
                    }
                    catch (IOException exception)
                    {
                        handleAppSerialError(exception);
                    }
                    kickoffRead!();
                }, null);
            };
            kickoffRead();

            while (!stoppingToken.IsCancellationRequested)
                await Task.Delay(100, stoppingToken);
        }

        public async Task Initialize(CancellationToken stoppingToken)
        {
            while (_serialMaxRetries > -1)
            {
                try
                {
                    _serialProfile.Open();
                    if (_serialProfile.IsOpen)
                        try { StartChannel(stoppingToken).Wait(stoppingToken); }
                        catch (Exception exception)
                        {
                            _logger.LogDebug($"Error: {exception.Message}");
                        }
                }
                catch (Exception exception)
                {
                    if (stoppingToken.IsCancellationRequested) return;
                    var retryLogOutput = $"retrying now, {_serialMaxRetries} remaining.";
                    _logger.LogError($"Connection failed {retryLogOutput}");
                    _logger.LogDebug($"Error: {exception.Message}");

                    if (isDevelopment && _serialMaxRetries == 0)
                    {
                        _logger.LogDebug("Switching to byte simulation mode...");
                        await Task.Run(async () =>
                        {
                            while (!stoppingToken.IsCancellationRequested)
                            {
                                await Task.Run(() => _serial.ManuallyReadData(_serialProfile, _selectedProtocol), stoppingToken);
                                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                            }
                        }, stoppingToken);

                        while (!stoppingToken.IsCancellationRequested)
                            await Task.Delay(100, stoppingToken);
                    }
                    else
                    {
                        if (_serialMaxRetries == 0)
                        {
                            _logger.LogError("Max retries reached, terminating...");
                            Environment.Exit(0);
                        }
                        if (_serialProfile.IsOpen)
                            try { StartChannel(stoppingToken).Wait(stoppingToken); }
                            catch (Exception ex)
                            {
                                _logger.LogDebug($"Error: {ex.Message}");
                            }
                    }
                }
                _serialMaxRetries--;
                await Task.Delay(_config.IOT_SERIAL_CONN_TIMEOUT, stoppingToken);
            }
        }
    }
}
