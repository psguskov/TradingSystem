using System.Collections.Generic;
using TradingPlatform;

namespace TradingSystem
{
    public interface IPowerTradesManager
    {
        void Validate(IEnumerable<PowerTrade> powerTrades);
        PowerTrade Aggregate(IEnumerable<PowerTrade> powerTrades);
        IEnumerable<PowerTrade> MapTrades(IEnumerable<Trade> trades);
    }
}