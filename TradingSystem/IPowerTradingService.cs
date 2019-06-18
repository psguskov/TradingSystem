using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingPlatform;

namespace TradingSystem
{
    public interface IPowerTradingService
    {
        IEnumerable<Trade> GetTrades(DateTime date);
        Task<IEnumerable<Trade>> GetTradesAsync(DateTime date);
    }
}
