using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace CommonDataLayer.DataAccess
{
    internal static class Configuration
    {
        const string DEFAULT_CONNECTION_KEY = "defaultConnection";

        public static string DefaultConnection
        {
            get
            {
                return ConfigurationManager.AppSettings[DEFAULT_CONNECTION_KEY];
            }
        }

        public static string ProviderName
        {
            get
            {
                return ConfigurationManager.ConnectionStrings[DefaultConnection].ProviderName;
            }
        }

        public static string ConnectionString
        {
            get
            {
               // return "Server=192.168.2.212;Database=BISNowERP;User Id=sa;Password=optimal$2009;";
                return ConfigurationManager.ConnectionStrings[DefaultConnection].ConnectionString;
            }
        }

        public static string GetConnectionString(string connectionName)
        {
            return ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        }

        public static string GetProviderName(string connectionName)
        {

            return Convert.ToString(ConfigurationManager.ConnectionStrings[connectionName].ProviderName);
        }

    }
}
