using RabbitMQ.Client;
using Vis.Common;
using Vis.Common.Models;
using Vis.Common.Models.Messages;

namespace Vis.Client.Publishers
{
    class DiskPublisher : IPublisher
    {
        public void send<TMessage>(string exchange, string routingKey, TMessage message)
        {
            //save to database
        }
        
    }
}
