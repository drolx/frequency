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
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RFIDIOT.Helpers;
using RFIDIOT.Models;

namespace RFIDIOT.Data
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


        public async Task<Scan> GetScanById(CaptureContext _context, Guid getbyid)
        {
            var ScanData = _context.Scans
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == getbyid);
            Task.WaitAll(ScanData);
            await ScanData;
            
            var Reader = _context.Readers
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == ScanData.Result.ReaderId);
            Task.WaitAll(Reader);
            await Reader;
            
            var Antenna = _context.Antennae
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == ScanData.Result.AntennaId);
            Task.WaitAll(Antenna);
            
            var Tag = _context.Tags
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == ScanData.Result.TagId);
            Task.WaitAll(Tag);
            await Tag;
            ScanData.Result.Reader = Reader.Result;
            ScanData.Result.Antenna = Antenna.Result;
            ScanData.Result.Tag = Tag.Result;
            
            return ScanData.Result;
        }

        public async Task<Scan> ResolveReader(CaptureContext _context, Scan _Scan, Reader _Reader)
        {
            var reader = _context.Readers
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.UniqueId == _config.IOT_UNIQUE_ID);
            Task.WaitAll(reader);
            await reader;
            if (reader.Result != null)
            {
                // Get already entered Unique Id for Reader.
                _Scan.ReaderId = reader.Result.Id;
            }
            else
            {
                // Enter new Reader details.
                // TODO: Make protocol and mode detection more dynamic.
                _Scan.Reader = _Reader;
            }

            return _Scan;
        }
        
        public async Task<Scan> ResolveAntenna(CaptureContext _context, Scan _Scan,  Antenna _Antenna, string atnData)
        {
            var antenna = _context.Antennae
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.UniqueId == atnData);
            Task.WaitAll(antenna);
            await antenna;
            if (antenna.Result != null)
            {
                // Get already entered Unique-Id for Antenna.
                _Scan.AntennaId = antenna.Result.Id;
            }
            else
            {
                _Scan.Antenna = _Antenna;
            }

            return _Scan;
        }

        public async Task<Scan> ResolveTag(CaptureContext _context, Scan _Scan, Tag _Tag, string _tagData)
        {
            var tag = _context.Tags
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.UniqueId == _tagData);
            Task.WaitAll(tag);
            await tag;
            
            if (tag.Result != null)
            {
                // Get already entered Unique Id for Tag.
                _Scan.TagId = tag.Result.Id;
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
