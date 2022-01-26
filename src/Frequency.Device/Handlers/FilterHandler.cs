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
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Proton.Frequency.Device.Data;
using Proton.Frequency.Device.Helpers;
using Proton.Frequency.Device.Models;

namespace Proton.Frequency.Device.Handlers
{
    public class FilterHandler
    {
        private static ConfigKey _config;
        private readonly ILogger<FilterHandler> _logger;
        public FilterHandler(
            ILogger<FilterHandler> logger,
            ConfigKey config
        )
        {
            _config = config;
            _logger = logger;
        }

        public async Task<bool> EarlyFilter(CaptureContext _context, Scan scan)
        {
            var checkedUpdate = _context.Tags
                    .FirstOrDefaultAsync(e => e.Id == scan.TagId);
            await checkedUpdate;
            Task.WaitAll(checkedUpdate);

            var diffInSeconds = 0.0;
            if (checkedUpdate.Result != null)
            {
                diffInSeconds = DateTime.Now.Subtract(checkedUpdate.Result.LastUpdated).TotalSeconds;
            }

            var checkFilter = checkedUpdate.Result == null || diffInSeconds >= _config.IOT_MIN_REPEAT_FREQ;

            if (!checkFilter)
            {
                _logger.LogInformation("Filtered By Time..");
                return checkFilter;
            }

            return checkFilter;

        }
    }
}
