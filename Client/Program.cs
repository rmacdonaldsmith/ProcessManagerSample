using System;
using System.IO;
using System.Threading.Tasks;
using log4net.Config;
using MassTransit;
using MassTransit.Log4NetIntegration;
using MassTransit.Log4NetIntegration.Logging;
using Messages;

namespace Client
{
    class Program
    {
        private const string HostAddress = "rabbitmq://ci-rmq-01.qasql.opentable.com";

        static void Main(string[] args)
        {
            ConfigureLog4Net();

            Console.WriteLine("Starting the Client...");

            var bus = Bus.Factory.CreateUsingRabbitMq(configurator =>
            {
                var hostUri = new Uri(HostAddress);
                var host = configurator.Host(hostUri, h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                configurator.UseLog4Net();
            });

            var busHandle = bus.Start();

            Console.WriteLine("Started. Press any key to start a redemption process...");
            Console.ReadLine();

            long gpid = 1233;
            var redemptionId = Guid.NewGuid();

            var sendAddress = new Uri(HostAddress + "/redemption_service");
            bus.GetSendEndpoint(sendAddress).ContinueWith(
                             task =>
                             task.Result.Send(new Commands.RedeemOtGiftCard(redemptionId, 1000, gpid, "USD", "rob@opentable.com")),
                             TaskContinuationOptions.NotOnFaulted);

            Console.WriteLine("Redemption sent [{1}] for Gpid [{2}]. Listening on [{0}]", HostAddress, redemptionId, gpid);
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
            busHandle.Stop();
        }

        static void ConfigureLog4Net()
        {
            string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log4Net.config");
            var configFile = new FileInfo(file);
            if (configFile.Exists)
                XmlConfigurator.ConfigureAndWatch(configFile);
            else
                BasicConfigurator.Configure();

            Log4NetLogger.Use();
        }
    }
}
