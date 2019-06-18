using AutoFixture;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingPlatform;
using TradingSystem;

namespace TradingSystemTests
{
    [TestFixture]
    public class PowerTradesManagerTests
    {
        IPowerTradesManager _manager;

        public PowerTradesManagerTests()
        {
            _manager = new PowerTradesManager();
        }

        [Test]
        public void ValidatePowerTradeCollection_CommonPeriodsCount_Expect_NoException()
        {
            // Arrange
            var powerTrades = TestHelper.BuildPowerTradesCollection();

            // Assert
            Assert.DoesNotThrow(() => _manager.Validate(powerTrades));
        }

        [Test]
        public void ValidatePowerTradeCollection_DifferentPeriodsCount_Expect_Exception()
        {
            // Arrange
            var tradesCount = TestHelper.GenerateIntFrom1To99();
            var periodsCount = TestHelper.GenerateIntFrom1To99();
            var date = new Fixture().Create<DateTime>();
            var powerTrades = TestHelper.BuildPowerTradesCollection(tradesCount, periodsCount, date);
            powerTrades.FirstOrDefault().Volumes = new Fixture().CreateMany<double>(periodsCount - 1).ToArray();

            // Assert
            Assert.Throws(typeof(ArgumentException), () => _manager.Validate(powerTrades));
        }

        [Test]
        public void ValidatePowerTradeCollection_DifferentDates_Expect_Exception()
        {
            // Arrange
            var tradesCount = TestHelper.GenerateIntFrom1To99();
            var periodsCount = TestHelper.GenerateIntFrom1To99();
            var date = new Fixture().Create<DateTime>();
            var powerTrades = TestHelper.BuildPowerTradesCollection(tradesCount, periodsCount, date);
            powerTrades.FirstOrDefault().Date = date.AddDays(-1);

            // Assert
            Assert.Throws(typeof(ArgumentException), () => _manager.Validate(powerTrades));
        }

        // TODO
        [Test]
        public void GetTradesByDate_Expect_NoException()
        {
            // Arrange
            var tradesCount = 1;
            var periodsCount = TestHelper.GenerateIntFrom1To99();
            var date = new Fixture().Create<DateTime>();
            var trades = TestHelper.BuildTradesCollection(tradesCount, periodsCount, date);

            // Act
            var powerTradesResult = _manager.MapTrades(trades);

            // Assert
            foreach (var powerTrade in powerTradesResult)
            {
                foreach (var trade in trades)
                {
                    Assert.AreEqual(powerTrade.Date, trade.Date);
                    Assert.AreEqual(powerTrade.Volumes.Length, trade.Periods.Length);
                    for (int i = 0; i < trade.Periods.Length; i++)
                    {
                        Assert.AreEqual(powerTrade.Volumes[i], trade.Periods[i].Volume);
                    }
                }
            }
        }
    }
}
