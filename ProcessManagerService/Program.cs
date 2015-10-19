using System;
using System.IO;
using System.Threading;
using Automatonymous;
using Castle.Windsor;
using log4net.Config;
using MassTransit;
using MassTransit.Log4NetIntegration;
using MassTransit.Log4NetIntegration.Logging;
using MassTransit.Saga;
using MassTransit.Util;
using Messages;
using ProcessManager.Core;

namespace ProcessManagerService
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigureLog4Net();

            //var container = new WindsorContainer();
            //container.Install(new MassTransitIntaller());
            //var bus = container.Resolve<IBusControl>();
            var bus = Bus.Factory.CreateUsingRabbitMq(configurator =>
            {
                var uriBuilder = new UriBuilder("rabbitmq", "ci-rmq-01.qasql.opentable.com");
                var host = configurator.Host(uriBuilder.Uri, h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                configurator.ReceiveEndpoint(host, "redemption_process_manager",
                    e => e.StateMachineSaga(new PointsProcessManager(), new InMemorySagaRepository<SampleState>()));

                configurator.ReceiveEndpoint(host, "redemption_service",
                    e => e.Consumer<ApplicationService>());

                configurator.UseLog4Net();
            });

            using (var busHandle = bus.Start())
            {
                TaskUtil.Await(() => busHandle.Ready);
                Console.WriteLine("Bus started");

                Console.WriteLine("Sending Redeem Command...");
                bus.Publish<Commands.RedeemOtGiftCard>(
                    new Commands.RedeemOtGiftCard(Guid.NewGuid(), 1000, 1234, "USD", "rob@opentable.com"));

                Console.WriteLine("Press any key to exit...");
                Console.ReadLine();
            }
        }

        static void ConfigureLog4Net()
        {
            var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config");
            var configFile = new FileInfo(file);
            if (configFile.Exists)
                XmlConfigurator.ConfigureAndWatch(configFile);
            else
                BasicConfigurator.Configure();

            Log4NetLogger.Use();
        }
    }
}
