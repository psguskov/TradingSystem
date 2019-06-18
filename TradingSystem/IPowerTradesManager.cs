using System.Collections.Generic;

namespace TradingSystem
{
    public interface IPowerTradesManager
    {
        bool Validate(IEnumerable<PowerTrade> powerTrades);
        PowerTrade Aggregate(IEnumerable<PowerTrade> powerTrades);
    }
}