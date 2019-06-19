using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingPlatform;

namespace TradingSystem
{
    public class PowerTradesDataProvider : IPowerTradesDataProvider
    {
        private ITraidingPlatformConnector _tradingPlatformConnector;
        private IPowerTradesManager _powerTradesManager;

        public PowerTradesDataProvider(ITraidingPlatformConnector tradingPlatformConnector,
             IPowerTradesManager powerTradesManager)
        {
            _tradingPlatformConnector = tradingPlatformConnector;
            _powerTradesManager = powerTradesManager;
        }

        public IEnumerable<PowerTrade> GetPowerTrades(DateTime date)
        {
            try
            {
                var tradePositions = _tradingPlatformConnector.GetTrades(date);
                return _powerTradesManager.MapTrades(tradePositions);
            }
            catch (Exception e)
            {
                throw new Exception("Error when getting trades: " + e.Message);
            }
        }
    }
}
