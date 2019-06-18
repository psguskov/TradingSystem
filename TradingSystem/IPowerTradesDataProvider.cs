using System;
using System.Collections.Generic;

namespace TradingSystem
{
    public interface IPowerTradesDataProvider
    {
        IEnumerable<PowerTrade> GetData(DateTime date);
    }
}