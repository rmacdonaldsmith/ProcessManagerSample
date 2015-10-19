using System;
using System.Threading.Tasks;
using Automatonymous;
using Messages;

namespace ProcessManager.Core.Activities
{
    public class SendOrderGiftCardCommandActivity :
        Activity<SampleState>
    {
        private readonly Uri _giftServiceAddress;

        public SendOrderGiftCardCommandActivity(Uri giftServiceAddress)
        {
            _giftServiceAddress = giftServiceAddress;
        }

        public void Accept(StateMachineVisitor visitor)
        {
            visitor.Visit(this, activity => { });
        }

        public async Task Execute(BehaviorContext<SampleState> context, Behavior<SampleState> next)
        {
            MassTransit.ConsumeContext consumeContext = null;
            if (!context.TryGetPayload(out consumeContext))
            {
                throw new Exception("Could not obtain the consumer context to send the OrderGift command.");
            }

            await consumeContext.GetSendEndpoint(_giftServiceAddress)
                         .ContinueWith(
                             task =>
                             task.Result.Send(new Commands.OrderOtGiftCard(context.Instance.RedemptionId, context.Instance.UsersEmailAddress)),
                             TaskContinuationOptions.NotOnFaulted);
        }

        public async Task Execute<T>(BehaviorContext<SampleState, T> context, Behavior<SampleState, T> next)
        {
            MassTransit.ConsumeContext consumeContext = null;
            if (!context.TryGetPayload(out consumeContext))
            {
                throw new Exception("Could not obtain the consumer context to send the OrderGift command.");
            }

            await consumeContext.GetSendEndpoint(_giftServiceAddress)
                         .ContinueWith(
                             task =>
                             task.Result.Send(new Commands.OrderOtGiftCard(context.Instance.RedemptionId, context.Instance.UsersEmailAddress)),
                             TaskContinuationOptions.NotOnFaulted);
        }

        public Task Faulted<TException>(BehaviorExceptionContext<SampleState, TException> context, Behavior<SampleState> next) where TException : Exception
        {
            //log, send message to error queue
            throw new NotImplementedException("I have implemented what happens when an Activity fails during sending messages.");
        }

        public Task Faulted<T, TException>(BehaviorExceptionContext<SampleState, T, TException> context, Behavior<SampleState, T> next) where TException : Exception
        {
            //log, send message to error queue
            throw new NotImplementedException("I have implemented what happens when an Activity fails during sending messages.");
        }
    }
}
