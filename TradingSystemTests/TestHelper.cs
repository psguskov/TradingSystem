using AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingPlatform;
using TradingSystem;

namespace TradingSystemTests
{
    public static class TestHelper
    {
        public static int GenerateIntFrom1To99()
        {
            var fixture = new Fixture();
            var generator = fixture.Create<Generator<int>>();

            return generator.First(x => x > 0 && x < 100);
        }

        public static IEnumerable<PowerTrade> BuildPowerTradesCollection(int tradesCapacity, int periodsCapacity, DateTime date)
        {
            var powerTrades = new Fixture().CreateMany<PowerTrade>(tradesCapacity);
            
            foreach (var trade in powerTrades)
            {
                trade.Volumes = new Fixture().CreateMany<double>(periodsCapacity).ToArray();
                trade.Date = date;
            }
            return powerTrades;
        }

        public static IEnumerable<PowerTrade> BuildPowerTradesCollection()
        {
            var tradesCount = GenerateIntFrom1To99();
            var periodsCount = GenerateIntFrom1To99();
            var date = new Fixture().Create<DateTime>();
            return BuildPowerTradesCollection(tradesCount, periodsCount, date);
        }

        public static List<Trade> BuildTradesCollection(int tradesCapacity, int periodsCapacity, DateTime date)
        {
            var trades = new List<Trade>();

            for (int i = 0; i < tradesCapacity; i++)
            {
                var trade = Trade.Create(date, periodsCapacity);
                foreach (var p in trade.Periods)
                {
                    var volume = new Fixture().Create<double>();
                    p.Volume = volume;
                }

                trades.Add(trade);
            }
            return trades;
        }
    }
}
