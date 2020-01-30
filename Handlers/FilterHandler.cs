//
// FilterHandler.cs
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
using uhf_rfid_catch.Data;
using uhf_rfid_catch.Helpers;
using uhf_rfid_catch.Models;

namespace uhf_rfid_catch.Handlers
{
    public class FilterHandler
    {
        private static ConfigKey _config;
        private static MainLogger _logger;
        public FilterHandler()
        {
            _config = new ConfigKey();
            _logger = new MainLogger();
        }

        public bool EarlyFilter(CaptureContext _context, Scan scan)
        {
            var checkedUpdate = _context.Tags
                .FirstOrDefault(e => e.Id == scan.TagId);

            var diffInSeconds = 0.0;
            
            if (checkedUpdate != null)
            {
                diffInSeconds = DateTime.Now.Subtract(checkedUpdate.LastUpdated).TotalSeconds;
            }

            var checkFilter = checkedUpdate == null || diffInSeconds >= _config.IOT_MIN_REPEAT_FREQ;

            if (!checkFilter)
            {
                _logger.Trigger("Info", "Filtered By Time.");
                return checkFilter;
            }
           
            return checkFilter;

        }
    }
}
