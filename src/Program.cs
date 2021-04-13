using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Stats;
using devices;
using System.Configuration;
using System.Collections.Specialized;

namespace Server
{
    public class Program
    {
        public static string configstring = Environment.GetEnvironmentVariable("HOME") != null
                ? Environment.GetEnvironmentVariable("HOME").ToString() + "/site/config.json"
                : @"config.json";

        public static dynamic config = Newtonsoft.Json.JsonConvert.DeserializeObject(configstring); 
        
        public static string frontendPort = config["Frontend"];
        public static int current = 0;
        public static int max = 10;
        public static List<device> devices = new List<device>();

        static ThreadStart stats = new ThreadStart(getStats);
        static ThreadStart hostTS = new ThreadStart(host);
        static string[] tempargs;

        public static void host()
        {
            CreateHostBuilder(tempargs).Build().Run();
        }

        public static void getStats()
        {
            while(true)
            {
                statcollection.statsGenerator();
            }
        }

        public static async Task Main(string[] args)
        {
            tempargs = args;
            Thread hostT = new Thread(hostTS);
            hostT.Start();
            Thread statT = new Thread(stats);
            statT.Start();
            register.manageDevices.updateDeviceList();
            Console.WriteLine("added");
            await devicemessages.Event.ReceiveMessagesFromDeviceAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}