using System;
using Automatonymous;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using MassTransit;
using MassTransit.Log4NetIntegration;
using MassTransit.Logging;
using MassTransit.Saga;
using ProcessManager.Core;

namespace ProcessManagerService
{
    public class MassTransitIntaller :
        IWindsorInstaller
    {
        static readonly ILog Log = Logger.Get<PointsProcessManager>();

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(configurator =>
            {
                var uriBuilder = new UriBuilder("rabbitmq", "ci-rmq-01.qasql.opentable.com");
                var host = configurator.Host(uriBuilder.Uri, h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                configurator.ReceiveEndpoint(host, "redemption_process_manager",
                    e =>
                    {
                        e.StateMachineSaga(new PointsProcessManager(), new InMemorySagaRepository<SampleState>());
                        
                    });

                configurator.ReceiveEndpoint(host, "redemption_service",
                    e =>
                    {
                        e.Consumer<ApplicationService>();
                    });

                configurator.UseLog4Net();
            });

            container.Register(Component.For<IBusControl>().Instance(bus).LifestyleSingleton());
        }
    }
}
