using System.IO;
using System.Net;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NEO_Block_API.lib;

namespace NEO_Block_API
{
    public class Program
    {
        public static string local = "";
        public static string ChainID = "Zoro";

        public static void Main(string[] args)
        {
            mySqlHelper.conf = Settings.Default.MysqlConfig;
            local = Settings.Default.Url;
            ZoroHelper.ZoroUrl = Settings.Default.Url;
            BuildWebHost(args).Run();
        }

        //private static int getServerPort() {
        //    int serverPort = 0;

        //    //尝试从配置文件读取，失败则默认82
        //    try
        //    {
        //        var config = new ConfigurationBuilder()
        //       .AddInMemoryCollection()    //将配置文件的数据加载到内存中
        //       .SetBasePath(Directory.GetCurrentDirectory())   //指定配置文件所在的目录
        //       .AddJsonFile("mongodbsettings.json", optional: true, reloadOnChange: true)  //指定加载的配置文件
        //       .Build();    //编译成对象        

        //        serverPort = int.Parse(config["serverPort"].ToString());
        //    }
        //    catch
        //    {
        //        serverPort = 59908;
        //    }

        //    return serverPort;
        //}

        public static IWebHost BuildWebHost(string[] args) =>
             WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseKestrel(options =>
                {
                    options.Listen(IPAddress.Any, Settings.Default.ServerPort);
                    //options.Listen(IPAddress.Loopback, getServerPort(), listenOptions =>
                    //{
                    //    listenOptions.UseHttps("openssl.pfx", "test1234");
                    //});
                })
                .Build();
    }
}
