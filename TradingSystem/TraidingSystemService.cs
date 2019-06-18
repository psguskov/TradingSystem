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
        private IScheduler Scheduler { get; }

        public TraidingSystemService(IScheduler scheduler) =>
            Scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));

        public void OnStart()
        {
            IJobDetail job = JobBuilder
                    .Create<ReportingJob>()
                    .WithIdentity(typeof(ReportingJob).Name, SchedulerConstants.DefaultGroup)
                    .Build();

            // Schedule - run every N minutes
            var schedule = $"0 0/{ReporterConfiguration.ReportingInterval} * * * ?"; 

            ITrigger trigger = TriggerBuilder
                                .Create()
                                .WithIdentity("simpletrigger", SchedulerConstants.DefaultGroup)
                                .WithCronSchedule(schedule)
                                .StartAt(DateTime.Now)
                                .Build();

            Scheduler.ScheduleJob(job, trigger);
            Scheduler.Start();
        }

        public void OnPaused() =>
            Scheduler.PauseAll();

        public void OnContinue() =>
            Scheduler.ResumeAll();

        public void OnStop() =>
            Scheduler.Shutdown();
    }
}
