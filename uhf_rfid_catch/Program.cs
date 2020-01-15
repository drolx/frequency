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
using System;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using uhf_rfid_catch.Handlers;
using uhf_rfid_catch.Handlers.ReaderConnections;

namespace uhf_rfid_catch
{
    class Program
    {
        private static void readerProcess()
        {
            ReaderConnection rdit = new ReaderConnection();
            rdit.Run();
            
            NetworkConnection ty = new NetworkConnection();
            ty.Boot();
        }
        
                static void Main(string[] args)
        {
            //Reader process thread//
            ////////////////////////
            Thread readerThread = new Thread(() => readerProcess());
            readerThread.Name = "UHF Reader Process";
            readerThread.Start();


            //Web view thread//
            //////////////////
            Thread webThread = new Thread(() => CreateHostBuilder(args).Build().Run());
            webThread.Name = "Web Process";
            webThread.Start();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}
