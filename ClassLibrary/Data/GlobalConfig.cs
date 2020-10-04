using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace ClassLibrary.Data
{
    public class GlobalConfig
    {
        public static string CnnString(string name = "DBConnectionString")
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
        public static string AppKeyLookup(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
