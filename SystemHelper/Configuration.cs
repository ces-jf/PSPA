using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace SystemHelper
{
    public class Configuration
    {
        public string ElasticSearchURL { get; set; }
        public static string DefaultTempBaseFiles { get { return Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempBases"); } }
        public static string DefaultTempFolder { get { return Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "_temp"); } }
        public double MaxMemoryUsing { get; set; }
        public static Version DatabaseVersion { get { return new Version(5, 5, 62); } }
        public int TokenMinutesValidation { get; set; }
    }
}
