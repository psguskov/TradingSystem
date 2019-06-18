using System;
using System.Collections.Generic;

namespace TradingSystem
{
    public interface IPowerTradesDataProvider
    {
        IEnumerable<PowerTrade> GetTrades(DateTime date);
    }
}