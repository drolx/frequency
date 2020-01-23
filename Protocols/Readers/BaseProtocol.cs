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
using uhf_rfid_catch.Models;

namespace uhf_rfid_catch.Protocols.Readers
{
    public abstract class BaseProtocol : IReaderProtocol
    {
        public BaseProtocol()
        {
        }
        // TODO: Optimize data type identification.
        // Set default byte length for auto stream mode.
        public virtual int AutoReadLength { get; set; } = 1;
        // Required if protocol doesn't have an auto detect mode.
        public virtual byte[] CommandReadTag { get; set; }
        // An extra option to specify if a protocol can auto read itself.
        public virtual bool DirectAutoRead { get; set; } = true;
        // Specify data type before processing response, in case conversion is required.
        public virtual string ProtocolDataType { get; set; } = "hex";
        // Received data converted to bytes
        public virtual byte[] ReceivedBytes { get; set; } = { };

        public virtual void Log()
        {
            Console.WriteLine($"***** Start decode/encode {ProtocolDataType} type session *****");
        }

        public virtual Scan DecodeData()
        {
            throw new NotImplementedException();
        }

        public virtual void Decode()
        {
            throw new NotImplementedException();
        }
    }
}
