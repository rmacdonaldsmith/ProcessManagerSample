using System;

namespace Messages
{
    public class Commands
    {
        public class RedeemOtGiftCard : IRedemptionMessage
        {
            public RedeemOtGiftCard(Guid redemptionId, int pointsToRedeem, long gpid, string currency, string usersEmailAddress)
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

        public class ReservePoints : IRedemptionMessage
        {
            public ReservePoints(int pointsToReserve, Guid redemptionId)
            {
                RedemptionId = redemptionId;
                PointsToReserve = pointsToReserve;
            }

            public int PointsToReserve { get; private set; }
            public Guid RedemptionId { get; private set; }
        }

        public class RollBackPointsReservation : IRedemptionMessage
        {
            public RollBackPointsReservation(int pointsToReserve, Guid redemptionId, string pointsExchangeId)
            {
                PointsExchangeId = pointsExchangeId;
                RedemptionId = redemptionId;
                PointsToReserve = pointsToReserve;
            }

            public int PointsToReserve { get; private set; }
            public Guid RedemptionId { get; private set; }
            public string PointsExchangeId { get; private set; }
        }

        public class CommitPoints : IRedemptionMessage
        {
            public CommitPoints(Guid redemptionId, string pointsExchangeId)
            {
                PointsExchangeId = pointsExchangeId;
                RedemptionId = redemptionId;
            }

            public Guid RedemptionId { get; private set; }
            public string PointsExchangeId { get; private set; }
        }

        public class OrderOtGiftCard : IRedemptionMessage
        {
            public OrderOtGiftCard(Guid redemptionId, string usersEmailAddress)
            {
                UsersEmailAddress = usersEmailAddress;
                RedemptionId = redemptionId;
            }

            public Guid RedemptionId { get; private set; }
            public string UsersEmailAddress { get; private set; }
        }
    }
}
