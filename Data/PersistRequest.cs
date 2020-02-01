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
using uhf_rfid_catch.Helpers;
using uhf_rfid_catch.Models;

namespace uhf_rfid_catch.Data
{
    public class PersistRequest
    {
        public readonly ConfigKey _config;
        public PersistRequest()
        {
            _config = new ConfigKey();
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

        public Scan ResolveReader(CaptureContext _context, Scan _Scan, Reader _Reader)
        {
            Reader readerid;
            if ((readerid = _context.Readers
                    .AsNoTracking()
                    .FirstOrDefault(e => e.UniqueId == _config.IOT_UNIQUE_ID)) != null)
            {
                // Get already entered Unique Id for Reader.
                _Scan.ReaderId = readerid.Id;
            }
            else
            {
                // Enter new Reader details.
                // TODO: Make protocol and mode detection more dynamic.
                _Scan.Reader = _Reader;
            }

            return _Scan;
        }

        public Scan ResolveTag(CaptureContext _context, Scan _Scan, Tag _Tag, string _tagData)
        {
            Tag tagid;
            if ((tagid = _context.Tags
                    .AsNoTracking()
                    .FirstOrDefault(e => e.UniqueId == _tagData)) != null)
            {
                // Get already entered Unique Id for Tag.
                _Scan.TagId = tagid.Id;
            }
            else
            {
                // Enter new Tag details.
                _Scan.Tag = _Tag;
            }

            return _Scan;
        }

    }
}
