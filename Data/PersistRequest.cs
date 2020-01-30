//
// PersistRequest.cs
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
using System.Linq;
using Microsoft.EntityFrameworkCore;
using uhf_rfid_catch.Models;

namespace uhf_rfid_catch.Data
{
    public class PersistRequest
    {

        public PersistRequest()
        {
        }
        
        public void OldestData()
        {

        }


        public Scan GetScanById(CaptureContext _context, Guid getbyid)
        {
            _context.Database.EnsureCreated();

            Scan ScanData = _context.Scans
                .AsNoTracking()
                .FirstOrDefault(e => e.Id == getbyid);
            ScanData.Reader = _context.Readers
                .AsNoTracking()
                .FirstOrDefault(e => e.Id == ScanData.ReaderId);
            ScanData.Tag = _context.Tags
                .AsNoTracking()
                .FirstOrDefault(e => e.Id == ScanData.TagId);
            return ScanData;
        }

        public Scan GetReaderById(Guid getbyid)
        {
            return null;
        }


        public Scan GetTagById(Guid getbyid)
        {
            return null;
        }

    }
}
