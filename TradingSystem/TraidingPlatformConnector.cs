using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingPlatform;

namespace TradingSystem
{
    /// <summary>
    /// Connector to 3rd-party datasource. 
    /// It is required to incapsulate communication with TradingService because of 
    /// ITradingService has internal protection level
    /// </summary>
    public class TraidingPlatformConnector : ITraidingPlatformConnector
    {

        TradingService _tradingService;

        public TraidingPlatformConnector()
        {
            _tradingService = new TradingService();
        }

        public IEnumerable<Trade> GetTrades(DateTime dateTime)
        {
            return _tradingService.GetTrades(dateTime);
        }
    }
}
