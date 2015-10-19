using System.Threading.Tasks;
using MassTransit;
using Messages;

namespace ProcessManager.Core
{
    public class ApplicationService :
        IConsumer<Commands.RedeemOtGiftCard>
    {
        public async Task Consume(ConsumeContext<Commands.RedeemOtGiftCard> context)
        {
            var command = context.Message;
            await context.Publish(new Events.RedemptionStarted(command.RedemptionId, command.PointsToRedeem, command.Gpid, command.Currency, command.UsersEmailAddress));
        }
    }
}
