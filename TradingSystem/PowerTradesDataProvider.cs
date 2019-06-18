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
        TradingService _tradingService;

        public PowerTradesDataProvider()
        {
            _tradingService = new TradingService();
        }

        public IEnumerable<PowerTrade> GetTrades(DateTime date)
        {
            try
            {
                var tradePositions = _tradingService.GetTrades(date);
                return MapTrades(tradePositions);
            }
            catch (Exception e)
            {
                throw new Exception("Error when getting trades: " + e.Message);
            }
        }

        private IEnumerable<PowerTrade> MapTrades(IEnumerable<Trade> trades)
        {
            var powerTrades = new List<PowerTrade>();

            foreach (var trade in trades)
            {
                var powerTraid = new PowerTrade(trade.Periods.Count());
                powerTraid.Date = trade.Date;
                foreach (var period in trade.Periods)
                {
                    powerTraid.AddVolumeByPeriod(period.Period, period.Volume);
                }
                powerTrades.Add(powerTraid);
            }
            return powerTrades;
        }
    }
}
