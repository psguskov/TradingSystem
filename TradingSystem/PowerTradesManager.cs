using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem
{
    public class PowerTradesManager : IPowerTradesManager
    {
        public bool Validate(IEnumerable<PowerTrade> powerTrades)
        {
            try
            {
                if (!powerTrades.Any())
                {
                    throw new Exception("There are no any trade positions");
                }

                if (powerTrades.Any(t => t.Volumes.Count() == 0))
                {
                    throw new Exception("Periods count must be more then zero");
                }

                if (!powerTrades.All(t => t.Volumes.Count() == powerTrades.First().Volumes.Count()))
                {
                    throw new Exception("Periods count must be the same for all trade positions");
                }

                return true;
            }
            catch (Exception e)
            {
                // TODO: Log an error
                return false;
            }
        }

        public PowerTrade Aggregate(IEnumerable<PowerTrade> powerTrades)
        {
            if (!powerTrades.Any())
            {
                throw new ArgumentException("Trades collection should not be empty");
            }

            int numberOfPeriods = powerTrades.First().Volumes?.Count() ?? 0;

            if (numberOfPeriods == 0)
            {
                throw new ArgumentException("Trade periods collection should not be empty");
            }

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
    }
}
