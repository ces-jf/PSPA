using System;
using System.Configuration;
using System.IO;

namespace SystemHelper
{
    public class Configuration
    {
        public string ElasticSearchURL { get; set; }
        public static string DefaultTempBaseFiles { get { return Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempBases"); } }
        public double MaxMemoryUsing { get; set; }
        public static Version DatabaseVersion { get { return new Version(10, 1, 38); } }
        public int TokenMinutesValidation { get; set; }
    }
}
