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
        private static void CallreaderThread()
        {
            ReaderConnection rdi = new ReaderConnection();
            rdi.Run();
        }
        
                static void Main(string[] args)
        {
            ThreadStart readerProcess = CallreaderThread;
                var readerThread = new Thread(readerProcess);
                readerThread.Name = "readerThread";
                readerThread.Start();
                

//            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}
