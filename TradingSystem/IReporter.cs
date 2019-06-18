using System;

namespace TradingSystem
{
    public interface IReporter
    {
        void GenerateReport(DateTime dateTime);
    }
}