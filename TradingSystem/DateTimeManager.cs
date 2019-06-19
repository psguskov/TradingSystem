using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem
{
    public static class DateTimeManager
    {
        public static DateTime CalculateReportingDate(DateTime utcTime, TimeSpan traidingDayStart)
        {
            return (utcTime.TimeOfDay >= traidingDayStart)
                ? utcTime.Date.AddDays(1)
                : utcTime.Date;
        }

        public static List<ReportPeriod> EnrichDataWithDates(PowerTrade aggregatedTrade)
        {
            var enrichedData = new List<ReportPeriod>();
            var beginningOfReportingDay = CalculateBegginingOfReportingDayUtc(aggregatedTrade.Date);
            for (int i = 0; i < aggregatedTrade.Volumes.Count(); i++)
            {
                var localDateTime = beginningOfReportingDay.Add(TimeSpan.FromHours(i));
                string timePeriod = localDateTime.TimeOfDay.ToString(@"hh\:mm");
                enrichedData.Add(new ReportPeriod { Period = timePeriod, Value = aggregatedTrade.Volumes[i] });
            }

            return enrichedData;
        }

        public static void ConvertPowerTradeDateTimeToLocal(PowerTrade powerTradeWithUtcDateTime)
        {
            var createdDateLocal = powerTradeWithUtcDateTime.CreatedDate.ToLocalTime();
            var dateLocal = powerTradeWithUtcDateTime.Date.ToLocalTime();
            powerTradeWithUtcDateTime.CreatedDate = createdDateLocal;
            powerTradeWithUtcDateTime.Date = dateLocal;
        }

        public static DateTime CalculateBegginingOfReportingDayUtc(DateTime dateTimeLocal)
        {
            var begginingOfReportingDayUtc = dateTimeLocal.AddDays(-1).Add(ReporterConfiguration.ReportingDayStartOffset);
            return begginingOfReportingDayUtc.ToUniversalTime();
        }
    }
}
