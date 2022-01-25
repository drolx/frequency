//
// BaseProtocol.cs
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
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Proton.Frequency.Terminal.Data;
using Proton.Frequency.Terminal.Helpers;
using Proton.Frequency.Terminal.Models;

namespace Proton.Frequency.Terminal.Protocols.Readers
{
    public abstract class BaseProtocol : IReaderProtocol
    {
        protected readonly ILogger<BaseProtocol> _logger;
        public readonly ByteAssist _assist;
        public readonly ConfigKey _config;
        public readonly SessionUtil _session;
        public CaptureContext _context;
        public readonly CapturePersist _persist;
        public readonly PersistRequest _request;

        // Data object declaration.
        protected Reader _Reader;
        protected Antenna _Antenna;
        protected Tag _Tag;
        protected Scan _Scan;

        protected string _tagData;
        protected string _tagType;

        public BaseProtocol(
            ILogger<BaseProtocol> logger,
            ByteAssist byteAssist,
            ConfigKey configKey,
            SessionUtil sessionUtil,
            CapturePersist capturePersist,
            CaptureContext context,
            PersistRequest persistRequest
        )
        {
            _logger = logger;
            _assist = byteAssist;
            _config = configKey;
            _session = sessionUtil;
            _persist = capturePersist;
            _context = context;
            _request = persistRequest;
        }

        // Default byte maps.
        public const byte START_RESPONSE_BYTE = 0x00;
        public const byte START_COMMAND_BYTE = 0x00;

        // Received data converted to bytes
        public virtual byte[] ReceivedData { get; set; } = { };

        // TODO: Optimize data type identification.
        // Set default byte length for auto stream mode.
        public virtual int DataLength { get; set; } = 1;

        // Length of custom information from the reader.
        public int TagDataLength { get; set; }

        // Required if protocol doesn't have an auto detect mode.
        public virtual byte[] RequestRead { get; set; }

        // An extra option to specify if a protocol can auto read itself.
        public virtual bool AutoRead { get; set; } = true;

        // Specify data type before processing response, in case conversion is required.
        public virtual string DataType { get; set; } = "hex";

        public virtual async Task Log()
        {
            _logger.LogInformation($"Received {DataType} data: {BitConverter.ToString(ReceivedData).Replace("-", string.Empty).ToLower()}");

            var decData = await DecodeData();
            if (await Persist(decData))
            {
                await using (var xcix = _context)
                {
                    try
                    {
                        var getFullScan = await _request.GetScanById(xcix, decData.Id);
                        if (getFullScan.Reader != null
                            && getFullScan.Tag != null
                            && getFullScan.Antenna != null)
                        {
                            var logVal = $"Reader-UID: {getFullScan.Reader.UniqueId} | " +
                                         $"Type: {getFullScan.Tag.Type} | " +
                                         $"Mode: {getFullScan.Reader.Mode} | " +
                                         $"Tag-UID: {getFullScan.Tag.UniqueId} | " +
                                         $"Ant-UID: {getFullScan.Antenna.UniqueId}";
                            _logger.LogInformation(logVal);
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogCritical(e.ToString().Remove(0, e.ToString().Length) + "Unique Ignored..");
                    }
                }
            }
        }

        public virtual Task<Scan> DecodeData()
        {
            throw new NotImplementedException();
        }

        public virtual async Task<bool> Persist(Scan data)
        {
            bool returnBool = await _persist.Save(data);
            return returnBool;
        }
    }
}
