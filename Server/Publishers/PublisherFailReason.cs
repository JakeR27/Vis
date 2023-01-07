using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal partial class Publishers
    {
        public enum PublisherFailReason
        {
            Unknown = -1,
            ChannelClosed = 0,
            ConnectionClosed = 1
        }

        public class PublisherFailException : Exception
        {
            public PublisherFailException(PublisherFailReason reason) : base($"Publish failed : {reason}") { }
        }
    }
}
