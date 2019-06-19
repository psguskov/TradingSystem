using System;
using System.Collections.Generic;
using AutoFixture;
using log4net;
using Moq;
using NUnit.Framework;
using TradingSystem;

namespace TradingSystemTests
{
    [TestFixture]
    public class ReporterTests
    {
        private Mock<IPowerTradesDataProvider> _powerTradesDataProviderMock;
        private Mock<IPowerTradesManager> _powerTradesManagerMock;
        private Mock<IPowerTradesReportExporter> _powerTradesReportExporterMock;
        private Mock<IReporterConfiguration> _reporterConfigurationMock;
        private Mock<ILog> _logMock;

        public ReporterTests()
        {
            _powerTradesDataProviderMock = new Mock<IPowerTradesDataProvider>();
            _powerTradesManagerMock = new Mock<IPowerTradesManager>();
            _powerTradesReportExporterMock = new Mock<IPowerTradesReportExporter>();
            _reporterConfigurationMock = new Mock<IReporterConfiguration>();
            _logMock = new Mock<ILog>();
        }

        [Test]
        public void GenerateReport_Expect_NoErrors()
        {
            var powerTrades = new Fixture().CreateMany<PowerTrade>();
            int periodsCount = new Fixture().Create<int>();
            var aggregatedPowerTrade = new Fixture().Create<PowerTrade>();
            var datetime = new Fixture().Create<DateTime>();

            // Arrange
            _powerTradesDataProviderMock.Setup(dp => dp.GetPowerTrades(It.IsAny<DateTime>())).Returns(powerTrades).Verifiable();
            _powerTradesManagerMock.Setup(m => m.Aggregate(It.IsAny<IEnumerable<PowerTrade>>())).Returns(aggregatedPowerTrade).Verifiable();
            _powerTradesManagerMock.Setup(m => m.Validate(It.IsAny<IEnumerable<PowerTrade>>())).Verifiable();
            _powerTradesReportExporterMock.Setup(re => re.Export(It.IsAny<PowerTrade>(), It.IsAny<string>())).Verifiable();
            _reporterConfigurationMock.Setup(c => c.GetReportingDayStartOffset()).Returns(It.IsAny<TimeSpan>());
            _reporterConfigurationMock.Setup(c => c.GetReportDirectory()).Returns(It.IsAny<string>());

            var reporter = new Reporter(_powerTradesDataProviderMock.Object, _powerTradesManagerMock.Object,
                _powerTradesReportExporterMock.Object, _reporterConfigurationMock.Object, _logMock.Object);

            // Act
            // Assert
            Assert.DoesNotThrow(() => reporter.GenerateReport(DateTime.UtcNow));
            _powerTradesDataProviderMock.Verify();
            _powerTradesManagerMock.Verify();
            _powerTradesReportExporterMock.Verify();
            
        }
    }
}
