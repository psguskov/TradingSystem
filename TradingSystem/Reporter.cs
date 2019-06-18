﻿using log4net;
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
                var tradesData = _powerTradesDataProvider.GetData(dateTime);

                var validationResult = _powerTradesManager.Validate(tradesData);
                if (validationResult == false)
                {
                    throw new Exception("Trade positions validation failed");
                }

                // Calculate result trades
                var aggregatedTrade = _powerTradesManager.Aggregate(tradesData);

                //Export result
                _powerTradesReportExporter.Export(aggregatedTrade);
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
