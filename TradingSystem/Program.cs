using Autofac;
using Autofac.Extras.Quartz;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;
using System.Reflection;
using System.Configuration;
using Topshelf.Logging;
using log4net;
using TradingPlatform;

namespace TradingSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            ContainerBuilder containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<TraidingSystemService>()
                            .AsSelf()
                            .InstancePerLifetimeScope();

            containerBuilder.RegisterModule(new QuartzAutofacFactoryModule
            {
                ConfigurationProvider = context =>
                    (NameValueCollection)ConfigurationManager.GetSection("quartzScheduler")
            });
            containerBuilder.RegisterModule(new QuartzAutofacJobsModule(typeof(ReportingJob).Assembly));

            containerBuilder.Register(c => LogManager.GetLogger(typeof(Object))).As<ILog>();

            //containerBuilder.RegisterType<TradingService>().As<IPowerTradingService>().SingleInstance();
            containerBuilder.RegisterType<PowerTradesDataProvider>().As<IPowerTradesDataProvider>().SingleInstance();
            containerBuilder.RegisterType<PowerTradesReportExporter>().As<IPowerTradesReportExporter>().SingleInstance();
            containerBuilder.RegisterType<PowerTradesManager>().As<IPowerTradesManager>().SingleInstance();
            containerBuilder.RegisterType<Reporter>().As<IReporter>().SingleInstance();

            IContainer container = containerBuilder.Build();

            HostFactory.Run(hostConfigurator =>
            {
                hostConfigurator.SetServiceName("TraidingSystemService");
                hostConfigurator.SetDisplayName("TraidingSystemService");
                hostConfigurator.SetDescription("The system performs traiding reports");

                hostConfigurator.RunAsLocalSystem();
                hostConfigurator.UseLog4Net();

                hostConfigurator.Service<TraidingSystemService>(serviceConfigurator =>
                {
                    serviceConfigurator.ConstructUsing(hostSettings => container.Resolve<TraidingSystemService>());

                    serviceConfigurator.WhenStarted(service => service.OnStart());
                    serviceConfigurator.WhenPaused(service => service.OnPaused());
                    serviceConfigurator.WhenContinued(service => service.OnContinue());
                    serviceConfigurator.WhenStopped(service => service.OnStop());
                });
            });
        }
    }
}
