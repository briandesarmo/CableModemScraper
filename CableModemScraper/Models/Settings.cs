using System;
using System.Configuration;

namespace CableModemScraper.Models
{
    internal class Settings
    {
        internal Uri BaseAddress { get; }
        internal string UserName { get; }
        internal string Password { get; }
        internal string StartupProceduresCsv { get; }
        internal string DownStreamCsv { get; }
        internal string UpStreamCsv { get; }

        public Settings()
        {
            BaseAddress = new Uri(ConfigurationManager.AppSettings[Constants.AppSettingsKeys.BaseAddress]);
            UserName = ConfigurationManager.AppSettings[Constants.AppSettingsKeys.UserName];
            Password = ConfigurationManager.AppSettings[Constants.AppSettingsKeys.Password];

            StartupProceduresCsv = ConfigurationManager.AppSettings[Constants.AppSettingsKeys.StartupProceduresCsv];
            DownStreamCsv = ConfigurationManager.AppSettings[Constants.AppSettingsKeys.DownstreamCsv];
            UpStreamCsv = ConfigurationManager.AppSettings[Constants.AppSettingsKeys.UpstreamCsv];
        }
    }
}
