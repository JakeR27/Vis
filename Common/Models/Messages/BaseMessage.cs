using System.Runtime.CompilerServices;

namespace Vis.Common.Models.Messages
{
    public abstract class BaseMessage
    {
        public long Id;
        public string DestinationExchange = "DEADBEEF";
        public string RoutingKey = "DEAD.BEEF";
    }
}
