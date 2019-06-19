using AutoFixture;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSystem;

namespace TradingSystemTests
{
    [TestFixture]
    public class DateTimeManagerTests
    {
        [TestCase("23:00", "2019-3-30 22:00", "2019-3-30 00:00")]
        [TestCase("23:00", "2019-3-30 23:00", "2019-3-31 00:00")]
        [TestCase("23:00", "2019-3-31 00:00", "2019-3-31 00:00")]
        [TestCase("23:00", "2019-3-31 01:00", "2019-3-31 00:00")]
        public void CalculateReportingDate_Expected_ReportingDate(string beginingOfDay, string dateNow, string reportingDay)
        {
            // Arrange
            TimeSpan beginingOfDayTimeSpan = TimeSpan.Parse(beginingOfDay, CultureInfo.InvariantCulture);
            DateTime now = DateTime.Parse(dateNow, CultureInfo.InvariantCulture);
            DateTime expextedResult = DateTime.Parse(reportingDay, CultureInfo.InvariantCulture);

            // Act
            DateTime actualResult = DateTimeManager.CalculateReportingDate(now, beginingOfDayTimeSpan);

            // Assert
            Assert.AreEqual(actualResult, expextedResult);
        }

        [Test]
        public void EnrichData_Expected_FirstTimePeriodIsBeginingOfTheDay()
        {
            // Arrange
            var powerTrade = new Fixture().Create<PowerTrade>();
            powerTrade.Date = powerTrade.Date.Date;

            // Act
            var result = DateTimeManager.EnrichDataWithDates(powerTrade);

            // Assert
            TimeSpan.TryParse(result.First().Period, out TimeSpan firstTimePeriod);
            var beginingOfDayUtc = DateTimeManager.CalculateBegginingOfReportingDayUtc(powerTrade.Date).TimeOfDay;
            Assert.AreEqual(firstTimePeriod, beginingOfDayUtc);
        }

        [TestCase("10/27/2019 00:00:00.000")] // London long day
        [TestCase("10/26/2014 00:00:00.000")] // Russia long day
        public void EnrichDataLongDay_Expected_LastPeriodIsNotDuplicateOfFirstPeriod(DateTime date)
        {
            // Arrange
            PowerTrade powerTrade;
            if (TimeZoneInfo.Local.IsDaylightSavingTime(date))
            {
                powerTrade = TestHelper.BuildPowerTradesCollection(1, 25, date).First();
            }
            else
            {
                powerTrade = TestHelper.BuildPowerTradesCollection(1, 24, date).First();
            }

            // Act
            var result = DateTimeManager.EnrichDataWithDates(powerTrade);

            // Assert
            Assert.AreNotEqual(result.First().Period, result.Last().Period);
        }
    }
}
