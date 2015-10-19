using System;
using MassTransit;

namespace Messages
{
    public interface IRedemptionMessage
    {
        Guid RedemptionId { get; }
    }

    public abstract class RedemptionBase : IRedemptionMessage, CorrelatedBy<Guid>
    {
        public Guid CorrelationId
        {
            get { return RedemptionId; }
        }
        public Guid RedemptionId { get; protected set; }
    }
}