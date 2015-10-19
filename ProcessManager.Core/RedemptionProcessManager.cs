using System;
using System.Globalization;
using Automatonymous;
using Automatonymous.Graphing;
using Messages;
using ProcessManager.Core.Activities;

namespace ProcessManager.Core
{
    public class SampleState :
        SagaStateMachineInstance
    {
        public Guid RedemptionId { get; set; }
        public long Gpid { get; set; }
        public string UsersEmailAddress { get; set; }
        public int NumberOfPoints { get; set; }
        public string PointsExchangeId { get; set; }

        public Guid CorrelationId { get; set; }
        public State CurrentState { get; set; }
    }

    public sealed class PointsProcessManager :
        MassTransitStateMachine<SampleState>
    {
        private readonly Uri _pointsVaultUri = new Uri("rabbitmq://ci-rmq-01.qasql.opentable.com/Messages:Commands-ReservePoints?durable=false&autodelete=true");

        public PointsProcessManager()
        {
            Initially(
                When(ReceivedRequestForNewRedemption)
                    .Then(context =>
                    {
                        context.Instance.Gpid = context.Data.Gpid;
                        context.Instance.RedemptionId = context.Data.RedemptionId;
                        context.Instance.NumberOfPoints = context.Data.PointsToRedeem;
                        context.Instance.UsersEmailAddress = context.Data.UsersEmailAddress;

                        Log(string.Format("redemptionid: {0}", context.Instance.RedemptionId));
                    })
                    .Add(new SendReservePointsCommandActivity(_pointsVaultUri))
                    .TransitionTo(Started)
                );

            During(Started,
                    When(PointsReservedEvent)
                        .Then(context =>
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("Entered PointsReserved.");
                        })
                        .TransitionTo(Completed)
                );

            Event(() => ReceivedRequestForNewRedemption,
                x => x.CorrelateById(context => context.Message.RedemptionId)
                    .SelectId(context => context.Message.RedemptionId));

            Event(() => PointsReservedEvent,
                x => x.CorrelateById(context => context.Message.RedemptionId)
                    .SelectId(context => context.Message.RedemptionId));

            InstanceState(x => x.CurrentState);
        }

        public State Started { get; set; }
        public State Completed { get; set; }

        public Event<Events.RedemptionStarted> ReceivedRequestForNewRedemption { get; set; }
        public Event<Events.PointsReserved> PointsReservedEvent { get; set; }

        private static void Log(string logMessage)
        {
            var fColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(logMessage);
            Console.ForegroundColor = fColor;
        }
    }
}
