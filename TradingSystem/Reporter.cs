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
        IPowerTradesDataProvider _powerTradesDataProvider;
        IPowerTradesManager _powerTradesManager;
        IPowerTradesReportExporter _powerTradesReportExporter;
        ILog _log;

        public Reporter(IPowerTradesDataProvider powerTradesDataProvider, 
                        IPowerTradesManager powerTradesManager,
                        IPowerTradesReportExporter powerTradesReportExporter,
                        ILog log)
        {
            _powerTradesDataProvider = powerTradesDataProvider;
            _powerTradesManager = powerTradesManager;
            _powerTradesReportExporter = powerTradesReportExporter;
            _log = log;
        }

        public void GenerateReport(DateTime dateTime)
        {
            try
            {
                _log.Info("Report generation started");

                var treportingDate = ReportingDayHelper.CalculateReportingDate(dateTime, ReporterConfiguration.ReportingDayStartOffset);

                var trades = _powerTradesDataProvider.GetPowerTrades(treportingDate);

                _powerTradesManager.Validate(trades);

                var aggregatedTrade = _powerTradesManager.Aggregate(trades);

                _powerTradesReportExporter.Export(aggregatedTrade, ReporterConfiguration.ReportDirectory);
                _log.Info("Report generation finished");
            }
            catch(Exception e)
            {
                _log.Error("Report generation failed", e.InnerException);
                throw;
            }
        }
    }
}
