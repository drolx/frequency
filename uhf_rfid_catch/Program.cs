using System;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using uhf_rfid_catch.Handlers;
using uhf_rfid_catch.Helpers;

namespace uhf_rfid_catch
{
    class Program
    {
          public static void CallToChildThread() {
            while(true) {
//                MainLogger mainLogger = new MainLogger();
//                mainLogger.Trigger("Info", "Testing log class-files");

//                ConfigContext settings = new ConfigContext();
//                String checkVal = settings.Resolve("testn1:testn2:testn3");
//                Console.WriteLine(checkVal);

                
                Console.WriteLine("**** child thread *****");
                Thread.Sleep(2500);
            }
         
      }
        static void Main(string[] args)
        {
            ThreadStart childref = CallToChildThread;
                Thread childThread = new Thread(childref);
                childThread.Start();
                
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}
