using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models.Messages
{
    public abstract class BaseMessage
    {
        public long Id;
        public string DestinationExchange = "DEADBEEF";
        public string RoutingKey = "DEAD.BEEF";
    }
}
