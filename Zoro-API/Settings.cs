using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Zoro_Web_API
{
    internal class Settings
    {
        public string MysqlConfig { get; }
        public string Url { get; }
        public static Settings Default { get; }
        public int ServerPort { get; }

        static Settings() {
            IConfigurationSection section = new ConfigurationBuilder().AddJsonFile("mysqlSettings.json").Build().GetSection("ApplicationConfiguration");
            Default = new Settings(section);
        }

        public Settings(IConfigurationSection section) {
            IEnumerable<IConfigurationSection> mysql = section.GetSection("MySql").GetChildren();
            MysqlConfig = "";
            foreach (var item in mysql)
            {
                MysqlConfig += item.Key + " = " + item.Value;
                MysqlConfig += ";";
            }
            Url = section.GetSection("UrL").Value;
            ServerPort = int.Parse(section.GetSection("ServerPort").Value);
        }
    }
}
