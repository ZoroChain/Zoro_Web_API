using System.IO;
using System.Net;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Zoro_Web_API.lib;

namespace Zoro_Web_API
{
    public class Program
    {
        public static string ChainID = "Zoro";

        public static void Main(string[] args)
        {
            mySqlHelper.conf = Settings.Default.MysqlConfig;
            ZoroHelper.ZoroUrl = Settings.Default.Url;
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
             WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseKestrel(options =>
                {
                    options.Listen(IPAddress.Any, Settings.Default.ServerPort);
                })
                .Build();
    }
}
