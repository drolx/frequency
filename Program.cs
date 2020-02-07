//
// Program.cs
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
using System.Threading;
using NLog.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using uhf_rfid_catch.Handlers;
using uhf_rfid_catch.Helpers;

namespace uhf_rfid_catch
{
    internal static class Program
    {
        private static readonly ConfigKey _config = new ConfigKey();
        private static readonly MainLogger _logger = new MainLogger();
        private static void readerProcess()
        {
            ReaderProcess _readerProcess = new ReaderProcess();
            _readerProcess.Run();
            
        }
        
                static void Main(string[] args)
        {
            _logger.Trigger("Info", "Booting up daemon.....");
            
            // Reader process thread//
            var readerThread = new Thread(readerProcess) {Name = "UHF Reader Process"};
            readerThread.Start();


            // Web view thread//
            if (_config.BASE_WEB_ENABLE)
            {
                var webThread = new Thread(() => CreateHostBuilder(args).Build().Run()) {Name = "Web Process"};
                webThread.Start();
            }
            
        }

                private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                .UseNLog();  // NLog: Setup NLog for Dependency injection
                }
}
