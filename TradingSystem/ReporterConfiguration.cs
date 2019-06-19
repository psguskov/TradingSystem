using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem
{
    public class ReporterConfiguration : IReporterConfiguration
    {
        public string GetReportDirectory()
        {
            var path = ConfigurationManager.AppSettings["directoryPath"];
            return !String.IsNullOrWhiteSpace(path) ? path
                : Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        }

        public int GetReportingInterval()
        {
            var intervalValue = ConfigurationManager.AppSettings["reportingIntervalInMinutes"];
            return (!string.IsNullOrWhiteSpace(intervalValue)
                && int.TryParse(intervalValue, out int interval)
                && interval > 0)
                ? interval
                : 5;
        }

        public TimeSpan GetReportingDayStartOffset()
        {
            var reportingDayStartValue = ConfigurationManager.AppSettings["reportingDayStartOffset"];
            return (!string.IsNullOrWhiteSpace(reportingDayStartValue)
                && TimeSpan.TryParse(reportingDayStartValue, out TimeSpan dayStart))
                ? dayStart
                : TimeSpan.FromHours(23);
        }
    }
}
