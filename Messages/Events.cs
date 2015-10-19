using System;

namespace Messages
{
    public class Events
    {
        public class RedemptionStarted : IRedemptionMessage
        {
            public RedemptionStarted(Guid redemptionId, int pointsToRedeem, long gpid, string currency, string usersEmailAddress)
            {
                UsersEmailAddress = usersEmailAddress;
                Currency = currency;
                Gpid = gpid;
                PointsToRedeem = pointsToRedeem;
                RedemptionId = redemptionId;
            }

            public Guid RedemptionId { get; private set; }
            public long Gpid { get; private set; }
            public int PointsToRedeem { get; private set; }
            public string Currency { get; private set; }
            public string UsersEmailAddress { get; private set; }
        }

        public class PointsReserved : IRedemptionMessage
        {
            public PointsReserved(Guid redemptionId, int reservedPoints, string pointsExchangeId)
            {
                PointsExchangeId = pointsExchangeId;
                ReservedPoints = reservedPoints;
                RedemptionId = redemptionId;
            }

            public Guid RedemptionId { get; private set; }
            public int ReservedPoints { get; private set; }
            public string PointsExchangeId { get; private set; }
        }

        public class PointsCommitted : IRedemptionMessage
        {
            public PointsCommitted(Guid redemptionId, int committedPoints, string pointsExchangeId)
            {
                PointsExchangeId = pointsExchangeId;
                CommittedPoints = committedPoints;
                RedemptionId = redemptionId;
            }

            public Guid RedemptionId { get; private set; }
            public int CommittedPoints { get; private set; }
            public string PointsExchangeId { get; private set; }
        }

        public class InsufficientPoints : IRedemptionMessage
        {
            public InsufficientPoints(Guid redemptionId, int pointsToRedeem)
            {
                PointsToRedeem = pointsToRedeem;
                RedemptionId = redemptionId;
            }

            public Guid RedemptionId { get; private set; }
            public int PointsToRedeem { get; private set; }
        }

        public class OtGiftCardOrderPlaced : IRedemptionMessage
        {
            public OtGiftCardOrderPlaced(Guid redemptionId)
            {
                RedemptionId = redemptionId;
            }

            public Guid RedemptionId { get; private set; }
        }
    }
}
