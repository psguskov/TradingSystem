using System;
using System.Collections.Generic;
using TradingPlatform;

namespace TradingSystem
{
    public interface ITraidingPlatformConnector
    {
        IEnumerable<Trade> GetTrades(DateTime dateTime);
    }
}