using System.Threading.Tasks;
using MassTransit;
using MassTransit.Logging;
using Messages;

namespace PointsVaultGateway
{
    public class CommandHandler : IConsumer<Commands.ReservePoints>
    {
        static readonly ILog Log = Logger.Get<CommandHandler>();

        public async Task Consume(ConsumeContext<Commands.ReservePoints> context)
        {
            Log.InfoFormat("Received request to reserve [{1}] points, RedemptionId [{0}]",
               context.Message.RedemptionId.ToString(), context.Message.PointsToReserve);

            if (context.Message.PointsToReserve < 900)
                await context.RespondAsync(new Events.InsufficientPoints(context.Message.RedemptionId, context.Message.PointsToReserve));
            else
                await context.RespondAsync(new Events.PointsReserved(context.Message.RedemptionId, context.Message.PointsToReserve, "points-exchange-01"));
        }
    }
}
