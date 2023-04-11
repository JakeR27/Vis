using System.Runtime.CompilerServices;

namespace Vis.Common.Models.Messages
{
    public abstract class BaseMessage
    {
        public Guid Id = Guid.NewGuid();
        public string DestinationExchange = "DEADBEEF";
        public string RoutingKey = "DEAD.BEEF";
    }
}
