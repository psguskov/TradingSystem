using log4net;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem
{
    public class ReportingJob : IJob
    {
        private ILog _log { get; }
        private IReporter _reporter;

        public ReportingJob(ILog log, IReporter reporter)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _reporter = reporter;
        }

        public Task Execute(IJobExecutionContext context) =>
            Task.Run(() => _reporter.GenerateReport(DateTime.Now));
    }
}
