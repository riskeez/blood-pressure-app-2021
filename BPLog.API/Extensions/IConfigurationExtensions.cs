using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPLog.API.Extensions
{
    /// <summary>
    /// Extension methods to simplify reading of the API settings
    /// </summary>
    public static class IConfigurationExtensions
    {
        /// <summary>
        /// Gets JWT private key from API configuration file
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static string GetPrivateKey(this IConfiguration configuration) => configuration.GetValue<string>("PrivateKey");

        /// <summary>
        /// Gets SQLite connection string from API configuration file
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static string GetSqliteConnection(this IConfiguration configuration) => configuration.GetConnectionString("SqliteConnection");
    }
}
