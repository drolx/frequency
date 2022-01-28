﻿//
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
using Tiny.RestClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Proton.Frequency.Device.Data;
using Proton.Frequency.Device.Helpers;
using Proton.Frequency.Device.Models;

namespace Proton.Frequency.Device.Handlers
{
    public class WebSync
    {
        private readonly ILogger<WebSync> _logger;
        private readonly ConfigKey _config;
        private readonly NetworkCheck _network;
        private readonly TinyRestClient _httpclient;
        private readonly CaptureContext _context;
        public WebSync(ILogger<WebSync> logger,
                ConfigKey config,
                HTTPInitializer httpInitializer,
                NetworkCheck network,
                CaptureContext captureContext
            )
        {
            _logger = logger;
            _config = config;
            _httpclient = httpInitializer.Resolve();
            _network = network;
            _context = captureContext;
            _context.PushStore = true;
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
                    checkStoreState = await _context.Scans
                        .Where(k => k.synced == 0)
                        .CountAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                await using (var context = _context)
                {
                    var isCompleted = false;
                    var pushCount = 0;

                    if (_config.IOT_REMOTE_HOST_ENABLE && netBool && checkStoreState > 0)
                    {
                        context.PushStore = true;
                    }

                    try
                    {
                        context.Database.EnsureCreated();
                    }
                    catch (Exception e)
                    {
                        _logger.LogCritical(e.ToString().Remove(0, e.ToString().Length) + "Bootstrapping DB now..");
                    }

                    var getLatest = context.Scans
                        .Where(k => k.synced == 0)
                        .Include(e => e.Reader)
                        .Include(y => y.Tag)
                        .Include(r => r.Antenna)
                        .Take(_config.IOT_MIN_REMOTE_PUSH_COUNT).AsEnumerable();

                    var totalDelete = getLatest.Count();
                    if (checkStoreState != 0)
                    {
                        _logger.LogInformation($"------>> Pushing {totalDelete} of {checkStoreState} cached data...");
                    }

                    List<Scan> updateList = new List<Scan>();
                    foreach (var innerData in getLatest)
                    {
                        if (innerData.Antenna != null)
                        {
                            var ScanData = new PostData
                            {
                                ReaderUID = innerData.Reader.UniqueId,
                                ReaderMode = innerData.Reader.Mode,
                                ReaderProtocol = innerData.Reader.Protocol,
                                AntennaUID = innerData.Antenna.UniqueId,
                                TagUID = innerData.Tag.UniqueId,
                                TagType = innerData.Tag.Type,
                                CaptureTime = innerData.CaptureTime.ToString()
                            };

                            switch (_config.IOT_REMOTE_HOST_METHOD)
                            {
                                case "GET":
                                    try
                                    {
                                        var request = _httpclient.GetRequest(string.Empty)
                                            .AsMultiPartFromDataRequest().
                                            AddContent(ScanData, "ScanData")
                                            .ExecuteAsHttpResponseMessageAsync();
                                        Task.WaitAll(request);
                                        isCompleted = request.IsCompletedSuccessfully;
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogError("Failed to post to cloud.." + ex.ToString().Remove(0, ex.ToString().Length));
                                    }

                                    break;
                                case "POST":

                                    try
                                    {
                                        var request = _httpclient.PostRequest(string.Empty)
                                            .AsMultiPartFromDataRequest().
                                            AddContent(ScanData, "ScanData")
                                            .ExecuteAsHttpResponseMessageAsync();
                                        Task.WaitAll(request);
                                        isCompleted = request.IsCompletedSuccessfully;
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogError("Error", "Failed to post to cloud.." + ex.ToString().Remove(0, ex.ToString().Length));
                                    }
                                    break;
                            }

                            if (isCompleted)
                            {
                                ++pushCount;
                                _logger.LogInformation($"*****   Pushed Scan {innerData.Id} from Tag: {innerData.Tag.UniqueId} to cloud  *****");
                                innerData.synced = 1;
                                updateList.Add(innerData);
                            }
                        }
                    }

                    if (pushCount == totalDelete)
                    {
                        if (_config.SERVER_FORWARD
                            && _config.SERVER_ENABLE
                            && _config.SERVER_STORE
                            && _config.IOT_REMOTE_HOST_ENABLE
                            && !_config.IOT_MODE_ENABLE
                            && _config.BASE_PERSIST_DATA)
                        {
                            context.Scans.UpdateRange(updateList);
                        }
                        else
                        {
                            context.Scans.RemoveRange(getLatest);
                        }
                        await context.SaveChangesAsync();
                    }
                }
            }
        }
    }
}