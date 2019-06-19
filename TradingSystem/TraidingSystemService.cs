using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem
{
    public class TraidingSystemService
    {
        private IScheduler _scheduler { get; }
        private IReporterConfiguration _reporterConfiguration;

        public TraidingSystemService(IScheduler scheduler, IReporterConfiguration reporterConfiguration)
        {
            _scheduler = scheduler;
            _reporterConfiguration = reporterConfiguration;
        }

        public void OnStart()
        {
            IJobDetail job = JobBuilder
                    .Create<ReportingJob>()
                    .WithIdentity(typeof(ReportingJob).Name, SchedulerConstants.DefaultGroup)
                    .Build();

            // Schedule - run every N minutes
            var interval = _reporterConfiguration.GetReportingInterval();
            var schedule = $"0 0/{interval} * * * ?";

            ITrigger trigger = TriggerBuilder
                                .Create()
                                .WithIdentity("simpletrigger", SchedulerConstants.DefaultGroup)
                                .WithCronSchedule(schedule)
                                .StartAt(DateTime.UtcNow)
                                .Build();

            _scheduler.ScheduleJob(job, trigger);
            _scheduler.Start();
        }

        public void OnPaused() =>
            _scheduler.PauseAll();

        public void OnContinue() =>
            _scheduler.ResumeAll();

        public void OnStop() =>
            _scheduler.Shutdown();
    }
}
