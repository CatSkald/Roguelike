using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace CatSkald.Roguelike.Host
{
    public static class Configuration
    {
        private static readonly IConfigurationRoot Root;

        static Configuration()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("AppSettings.json");

            Root = builder.Build();
        }

        public static string Get(string key)
        {
            return Root[key];
        }
    }
}
