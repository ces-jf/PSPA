using System;
using System.Configuration;
using System.IO;

namespace SystemHelper
{
    public class Configuration
    {
        public static string ElasticSearchURL { get { return ConfigurationManager.AppSettings["ElasticSearchURL"].ToString(); } }
        public static string DefaultTempBaseFiles { get { return Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempBases"); } }
        public static double MaxMemoryUsing { get { return double.Parse(ConfigurationManager.AppSettings["MaxMemoryUsing"].ToString()); } }
        public static Version DatabaseVersion { get { return new Version(10, 1, 38); } }
    }
}
