//
// BitUtil.cs
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
using System.Linq;

namespace uhf_rfid_catch.Helpers
{
    public class ByteAssist
    {
        public ByteAssist()
        {
        }
        
        public byte[] HexToByteArray(string hex) {
            return Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }

        public byte[] from(byte[] BytesArray, int StartFrom)
        {
            var TempFilter = BytesArray.Select((v, i) => new {Index = i, Value = v})
                .Where(k => k.Index >= StartFrom)
                .Select(y => y.Value);
            return TempFilter.ToArray();
        }

        public byte[] to(byte[] BytesArray, int StopAt)
        {
            var TempFilter = BytesArray.Select((v, i) => new {Index = i, Value = v})
                .Where(k => k.Index >= 0 && k.Index < StopAt)
                .Select(y => y.Value);
            return TempFilter.ToArray();
        }
        
        public byte[] between(byte[] BytesArray,int Start, int End)
        {
            var TempFilter = BytesArray.Select((v, i) => new {Index = i, Value = v})
                .Where(k => k.Index >= Start && k.Index <= End)
                .Select(y => y.Value);
            return TempFilter.ToArray();
        }
        
        public byte pick(byte[] BytesArray, int point)
        {
            var TempFilter = BytesArray.Select((v, i) => new {Index = i, Value = v})
                .Where(k => k.Index == point)
                .Select(y => y.Value);
            return TempFilter.ToArray()[0];
        }

    }
}
