using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem
{
    public static class ReportingHelper
    {
        public static DateTime CalculateReportingDate(DateTime utcTime, TimeSpan traidingDayStart)
        {
            return (utcTime.TimeOfDay >= traidingDayStart)
                ? utcTime.Date.AddDays(1)
                : utcTime.Date;
        }

        public static List<ReportPeriod> EnrichData(PowerTrade aggregatedTrade)
        {
            var enrichedData = new List<ReportPeriod>();
            var beginningOfReportingDay = CalculateBegginingOfReportingDay(aggregatedTrade.Date);
            for (int i = 0; i < aggregatedTrade.Volumes.Count(); i++)
            {
                string timePeriod = beginningOfReportingDay.Add(TimeSpan.FromHours(i)).TimeOfDay.ToString(@"hh\:mm");
                enrichedData.Add(new ReportPeriod { Period = timePeriod, Value = aggregatedTrade.Volumes[i] });
            }

            return enrichedData;
        }

        private static DateTime CalculateBegginingOfReportingDay(DateTime dateTime)
        {
            return dateTime.AddDays(-1).Add(ReporterConfiguration.ReportingDayStartOffset);
        }
    }
}
