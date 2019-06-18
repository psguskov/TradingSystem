using System;
using System.Collections.Generic;

namespace TradingSystem
{
    public interface IPowerTradesDataProvider
    {
        IEnumerable<PowerTrade> GetPowerTrades(DateTime date);
    }
}