using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Messages;

namespace Client
{
    public class EventHandler : IConsumer<Events.PointsCommitted>
    {
        public Task Consume(ConsumeContext<Events.PointsCommitted> context)
        {
            //Console.WriteLine("Points Committed!");

            throw new NotImplementedException();
        }
    }
}
