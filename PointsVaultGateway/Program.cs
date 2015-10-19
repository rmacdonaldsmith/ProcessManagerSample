using System;
using System.IO;
using log4net.Config;
using MassTransit;
using MassTransit.Log4NetIntegration;
using MassTransit.Log4NetIntegration.Logging;

namespace PointsVaultGateway
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigureLog4Net();

            const string rabbitHostAddress = "amqp://ci-rmq-01.qasql.opentable.com";
            Console.WriteLine("Starting the PointsGateway service...");

            var bus = Bus.Factory.CreateUsingRabbitMq(configurator =>
            {
                var host = configurator.Host(new Uri(rabbitHostAddress), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                configurator.ReceiveEndpoint(host, "points_service_queue",
                    e =>
                    {
                        e.Consumer<CommandHandler>();
                    });

                configurator.UseLog4Net();
            });

            var busHandle = bus.Start();

            Console.WriteLine("Bus started. Listening on [{0}]", rabbitHostAddress);
            Console.ReadLine();

            busHandle.Stop();
        }

        static void ConfigureLog4Net()
        {
            string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config");
            var configFile = new FileInfo(file);
            if (configFile.Exists)
                XmlConfigurator.ConfigureAndWatch(configFile);
            else
                BasicConfigurator.Configure();

            Log4NetLogger.Use();
        }
    }
}
