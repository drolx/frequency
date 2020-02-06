//
// CapturePersist.cs
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
using Microsoft.EntityFrameworkCore;
using uhf_rfid_catch.Handlers;
using uhf_rfid_catch.Models;

namespace uhf_rfid_catch.Data
{
    public class CapturePersist
    {
        private static FilterHandler _filter;
        private readonly MainLogger _logger;

        public CapturePersist()
        {
            _filter = new FilterHandler();
            _logger = new MainLogger();
        }

        public void OldestDelete()
        {

        }

        public async Task<bool> Save(Scan scn)
        {
            var returnBool = false;
            
            using (var _context = new CaptureContext())
            {
                if (await _filter.EarlyFilter(_context, scn))
                {
                    _context.Add(scn);
                    
                    var tagUpdate = _context.Tags
                        .FirstOrDefaultAsync(e => e.Id == scn.TagId);
                    Task.WaitAll(tagUpdate);
                    await tagUpdate;

                    if (scn.Tag != null && tagUpdate.Result != null)
                    {
                        tagUpdate.Result.LastUpdated = DateTime.Now;
                    }
                    
                    var antennaUpdate = _context.Antennae
                        .FirstOrDefaultAsync(e => e.Id == scn.AntennaId);
                    Task.WaitAll(antennaUpdate);
                    await antennaUpdate;

                    if (scn.Antenna != null && antennaUpdate.Result != null)
                    {
                        antennaUpdate.Result.LastUpdated = DateTime.Now;
                    }
                    
                    var readerUpdate = _context.Readers
                        .FirstOrDefaultAsync(e => e.Id == scn.ReaderId);
                    Task.WaitAll(readerUpdate);
                    await readerUpdate;

                    if (scn.Reader != null && readerUpdate.Result != null)
                    {
                        readerUpdate.Result.LastUpdated = DateTime.Now;
                    }

                    returnBool = true;
                }
                
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    returnBool = false;
                    if (!e.ToString().Contains("constraint failed"))
                    {
                        _logger.Trigger("Fatal", e.ToString());
                    }
                }

                return returnBool;
            }
        }
    }
}
