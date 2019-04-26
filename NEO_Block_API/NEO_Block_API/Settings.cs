using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NEO_Block_API
{
    internal class Settings
    {
        public string MysqlConfig { get; }
        public string Url { get; }
        public static Settings Default { get; }

        static Settings() {
            IConfigurationSection section = new ConfigurationBuilder().AddJsonFile("mysqlSettings.testnet.json").Build().GetSection("ApplicationConfiguration");
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
        }
    }
}
