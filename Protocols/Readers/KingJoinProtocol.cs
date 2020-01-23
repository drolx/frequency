//
// KingJoinProtocol.cs
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
using NLog.Fluent;
using uhf_rfid_catch.Models;

namespace uhf_rfid_catch.Protocols.Readers
{
    public class KingJoinProtocol : BaseProtocol
    {

        public KingJoinProtocol()
        {
            DataLength = 32;
        }
        
        public new const byte START_RESPONSE_BYTE = 0xCC;
        public new const byte START_COMMAND_BYTE = 0x7C;
        
        // Control identification data description
        public const byte ISO_SINGLE = 0x01;
        public const byte ISO_MEM = 0x02;
        
        public const byte CID1_EPC_SINGLE = 0x10;
        public const byte CID1_EPC_MULTI = 0x11;
        public const byte CID1_EPC_MEM = 0x12;
        
        // Control identification data actions.
        public const byte CID2_GET = 0x32;
        public const byte CID2_SET= 0x31;
        public const byte CID2_SUPER_GET = 0x22;
        public const byte CID2_SUPER_SET = 0x21;
        
        // Return code/status types
        public const byte RTN_SUCCESS = 0x00;
        public const byte RTN_FAIL = 0x01;
        public const byte RTN_AUTO = 0x32;

        public override Scan DecodeData()
        {
            return base.DecodeData();
        }
    }
}
