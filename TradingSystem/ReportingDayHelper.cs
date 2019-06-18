using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem
{
    public static class ReportingDayHelper
    {
        public static DateTime CalculateReportingDate(DateTime utcTime, TimeSpan traidingDayStart)
        {
            return (utcTime.TimeOfDay >= traidingDayStart)
                ? utcTime.Date.AddDays(1)
                : utcTime.Date;
        }
    }
}
