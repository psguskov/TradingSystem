using System;
using System.Collections.Generic;
using AutoFixture;
using log4net;
using Moq;
using NUnit.Framework;
using TradingPlatform;
using TradingSystem;

namespace TradingSystemTests
{
    [TestFixture]
    public class PowerTradesDataProviderTests
    {
        Mock<ITraidingPlatformConnector> _tradingPlatformConnectorMock;
        Mock<IPowerTradesManager> _powerTradesManagerMock;

        public PowerTradesDataProviderTests()
        {
            _tradingPlatformConnectorMock = new Mock<ITraidingPlatformConnector>();
            _powerTradesManagerMock = new Mock<IPowerTradesManager>();
        }

        [Test]
        public void GetTradesByDate_Expect_NoException()
        {
            var tradesCount = 1;
            var periodsCount = TestHelper.GenerateIntFrom1To99();
            var date = new Fixture().Create<DateTime>();
            var trades = TestHelper.BuildTradesCollection(tradesCount, periodsCount, date);
            var powerTrades = TestHelper.BuildPowerTradesCollection(tradesCount, periodsCount, date);

            // Arrange
            _tradingPlatformConnectorMock.Setup(c => c.GetTrades(It.IsAny<DateTime>())).Returns(trades).Verifiable();
            _powerTradesManagerMock.Setup(m => m.MapTrades(It.IsAny<IEnumerable<Trade>>()))
                .Returns(powerTrades).Verifiable();
            var dataProvider = new PowerTradesDataProvider(_tradingPlatformConnectorMock.Object, 
                _powerTradesManagerMock.Object);

            // Act
            // Assert
            Assert.DoesNotThrow(() => dataProvider.GetPowerTrades(date));
            _tradingPlatformConnectorMock.Verify();
            _powerTradesManagerMock.Verify();
        }
    }
}
