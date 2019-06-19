using System;

namespace TradingSystem
{
    public interface IReporterConfiguration
    {
        string GetReportDirectory();
        TimeSpan GetReportingDayStartOffset();
        int GetReportingInterval();
    }
}