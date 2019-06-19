using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem
{
    public class Reporter : IReporter
    {
        private IPowerTradesDataProvider _powerTradesDataProvider;
        private IPowerTradesManager _powerTradesManager;
        private IPowerTradesReportExporter _powerTradesReportExporter;
        private IReporterConfiguration _reporterConfiguration;
        private ILog _log;

        public Reporter(IPowerTradesDataProvider powerTradesDataProvider, 
                        IPowerTradesManager powerTradesManager,
                        IPowerTradesReportExporter powerTradesReportExporter,
                        IReporterConfiguration reporterConfiguration,
                        ILog log)
        {
            _powerTradesDataProvider = powerTradesDataProvider;
            _powerTradesManager = powerTradesManager;
            _powerTradesReportExporter = powerTradesReportExporter;
            _reporterConfiguration = reporterConfiguration;
            _log = log;
        }

        public void GenerateReport(DateTime dateTimeUtc)
        {
            _log.Info("Report generation started");

            var reportingDate = DateTimeManager.CalculateReportingDate(dateTimeUtc, 
                _reporterConfiguration.GetReportingDayStartOffset());

            var trades = _powerTradesDataProvider.GetPowerTrades(reportingDate);

            _powerTradesManager.Validate(trades);

            var aggregatedTrade = _powerTradesManager.Aggregate(trades);

            _powerTradesReportExporter.Export(aggregatedTrade, _reporterConfiguration.GetReportDirectory());

            _log.Info("Report generation finished");
        }
    }
}
