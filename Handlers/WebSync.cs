//
// WebSync.cs
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
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tiny.RestClient;
using uhf_rfid_catch.Data;
using uhf_rfid_catch.Helpers;
using uhf_rfid_catch.Models;

namespace uhf_rfid_catch.Handlers
{
    public class WebSync
    {
        private readonly MainLogger _logger;
        private readonly ConsoleLogger _consolelog;
        private readonly ConfigKey _config;
        private readonly NetworkCheck _network;
        private TinyRestClient _httpclient;
        private readonly CaptureContext _context;
        public WebSync()
        {
            _logger = new MainLogger();
            _consolelog = new ConsoleLogger();
            _config = new ConfigKey();
            _network = new NetworkCheck();
            _context = new CaptureContext();
            _context.PushStore = true;
            
            // Initialize HTTP calls
            _httpclient = new TinyRestClient(new HttpClient(), _config.IOT_REMOTE_HOST_URL);
            _httpclient.Settings.DefaultTimeout = TimeSpan.FromSeconds(_config.IOT_MIN_REMOTE_HOST_DELAY);
            _httpclient.Settings.DefaultHeaders.Add("X-IOT", "by gpproton...");

            if (!String.IsNullOrEmpty(_config.IOT_REMOTE_HOST_USERNAME)
            && !String.IsNullOrEmpty(_config.IOT_REMOTE_HOST_PASSWORD))
            {
                _httpclient.Settings.DefaultHeaders.AddBasicAuthentication(_config.IOT_REMOTE_HOST_USERNAME, _config.IOT_REMOTE_HOST_PASSWORD);
            }
            
        }

        public async Task Sync()
        {
            int checkStoreState = 0;
            var netBool = _network.Status();
            if (netBool)
            {
                try
                {
                    _context.Database.EnsureCreated();
                    checkStoreState = await _context.Scans.CountAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            await using (var context = new CaptureContext())
            {
                var isCompleted = false;

                if (netBool && checkStoreState > 0)
                {
                    context.PushStore = true;
                    _logger.Trigger("Info", $"------>> Pushed 1 of {checkStoreState} cached data...");
                }
                
                try
                {
                    context.Database.EnsureCreated();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                
                var getLatest = context.Scans
                    .Select(e => new {e, e.Reader, e.Antenna, e.Tag})
                    .ToList();

                    foreach (var datav in getLatest)
                    {
                        if (_config.IOT_REMOTE_HOST_ENABLE && netBool && datav != null)
                        {
                            var ScanData = new PostData
                            {
                                ReaderUID = datav.Reader.UniqueId,
                                ReaderMode = datav.Reader.Mode,
                                ReaderProtocol = datav.Reader.Protocol,
                                AntennaUID = datav.Antenna.UniqueId,
                                TagUID = datav.Tag.UniqueId,
                                TagType = datav.Tag.Type,
                                CaptureTime = datav.e.CaptureTime.ToString()
                            };
                        
                            switch (_config.IOT_REMOTE_HOST_METHOD)
                            {
                                case "GET":
                                    try
                                    {
                                        var Request = _httpclient.GetRequest(string.Empty)
                                            .AsMultiPartFromDataRequest().
                                            AddContent(ScanData, "ScanData")
                                            .ExecuteAsHttpResponseMessageAsync();
                                        Task.WaitAll(Request);
                                        isCompleted = Request.IsCompletedSuccessfully;
                                    }
                                    catch (Exception ex)
                                    {
                                        var excp = ex.ToString();
                                        excp = string.Empty;
                                        _logger.Trigger("Error", "Failed to post to cloud.." + excp);
                                    }

                                    break;
                                case "POST":
                                
                                    try
                                    {
                                        var Request = _httpclient.PostRequest(string.Empty)
                                            .AsMultiPartFromDataRequest().
                                            AddContent(ScanData, "ScanData")
                                            .ExecuteAsHttpResponseMessageAsync();
                                        Task.WaitAll(Request);
                                        isCompleted = Request.IsCompletedSuccessfully;
                                    }
                                    catch (Exception ex)
                                    {
                                        var excp = ex.ToString();
                                        excp = string.Empty;
                                        _logger.Trigger("Error", "Failed to post to cloud.." + excp);
                                    }
                                    break;
                            }

                            var forDelete = context.Scans
                                .FirstOrDefaultAsync(e => e.Id == datav.e.Id);
                            Task.WaitAll(forDelete);
                            context.Scans.Remove(forDelete.Result);

                            Task.WaitAll();
                        if (isCompleted && await context.SaveChangesAsync() == 1)
                        {
                            _consolelog.Trigger("Info",
                                $"*****   Pushed Scan {datav.e.Id} from Tag: {datav.Tag.UniqueId} to cloud  *****");
                        }
                        }
                }
            }
        }
    }
}
