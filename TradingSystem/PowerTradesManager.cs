using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingPlatform;

namespace TradingSystem
{
    public class PowerTradesManager : IPowerTradesManager
    {
        public void Validate(IEnumerable<PowerTrade> powerTrades)
        {
            if (!powerTrades.Any())
            {
                throw new ArgumentException("Trades collection should not be empty", "PowerTrades");
            }

            if (powerTrades.Any(t => t.Volumes.Count() == 0))
            {
                throw new ArgumentException("Trade periods collection should not be empty", "Volumes");
            }

            if (!powerTrades.All(t => t.Volumes.Count() == powerTrades.First().Volumes.Count()))
            {
                throw new ArgumentException("Periods count must be the same for all trade positions", "Volumes");
            }

            if (!powerTrades.All(t => t.Date == powerTrades.First().Date))
            {
                throw new ArgumentException("Dates must be the same for all trade positions", "Date");
            }
        }

        public PowerTrade Aggregate(IEnumerable<PowerTrade> powerTrades)
        {
            int numberOfPeriods = powerTrades.First()?.Volumes?.Count() ?? 0;

            var tradeDate = powerTrades.First().Date;

            var resultTrade = new PowerTrade(numberOfPeriods);
            resultTrade.Date = tradeDate;
            foreach (var powerTrade in powerTrades)
            {
                for (int i = 0; i < powerTrade.Volumes.Count(); i++)
                {
                    resultTrade.Volumes[i] += powerTrade.Volumes[i];
                }
            }
            return resultTrade;
        }

        public void AddVolumeByPeriod(double[] volumes, int period, double volume)
        {
            if (period < 1 || period > volumes.Length)
            {
                throw new ArgumentException("Incorrect period", "Period");
            }
            volumes[period - 1] += volume;
        }

        public  IEnumerable<PowerTrade> MapTrades(IEnumerable<Trade> trades)
        {
            var powerTrades = new List<PowerTrade>();

            foreach (var trade in trades)
            {
                var powerTraid = new PowerTrade(trade.Periods.Count());
                powerTraid.Date = trade.Date;
                foreach (var period in trade.Periods)
                {
                    AddVolumeByPeriod(powerTraid.Volumes, period.Period, period.Volume);
                }
                powerTrades.Add(powerTraid);
            }
            return powerTrades;
        }
    }
}
