using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem
{
    public static class ReporterConfiguration
    {
        private static string ReportDirectoryDefault => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        private static int ReportingIntervalDefault => 5;
        private static TimeSpan ReportingDayStartDefault => TimeSpan.FromHours(23);

        public static string ReportDirectory => GetReportDirectory();
        public static int ReportingInterval => GetReportingInterval();
        public static TimeSpan ReportingDayStartOffset => GetReportingDayStartOffset();

        private static string GetReportDirectory()
        {
            var path = ConfigurationManager.AppSettings["directoryPath"];
            return !String.IsNullOrWhiteSpace(path) ? path : ReportDirectoryDefault;

        }

        private static int GetReportingInterval()
        {
            var intervalValue = ConfigurationManager.AppSettings["reportingIntervalInMinutes"];
            return (!string.IsNullOrWhiteSpace(intervalValue)
                && int.TryParse(intervalValue, out int interval)
                && interval > 0)
                ? interval
                : ReportingIntervalDefault;
        }

        private static TimeSpan GetReportingDayStartOffset()
        {
            var reportingDayStartValue = ConfigurationManager.AppSettings["reportingDayStartOffset"];
            return (!string.IsNullOrWhiteSpace(reportingDayStartValue)
                && TimeSpan.TryParse(reportingDayStartValue, out TimeSpan dayStart))
                ? dayStart
                : ReportingDayStartDefault;
        }
    }
}
