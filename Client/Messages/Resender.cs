using Vis.Client.Database;
using Vis.Common;
using Vis.Common.Models;
using Vis.Common.Models.Messages;
using Vis.Common.Startup;

namespace Vis.Client.Messages;

public class Resender : BaseStartupTask
{
    protected override string _taskDescription { get; } = "Handles the resending of previously failed messages";

    public Resender()
    {
        ReadyState = State.COMPLETE;
    }
    protected override void _execute()
    {
        Logs.LogDebug("IN RESENDER");
        var messages = LiteDb.Instance.Dbo.GetCollection<LiteDb.Wrapper<BaseMessage>>("failed_messages").FindAll();
        foreach (var message in messages)
        {
            
            Logs.LogDebug(message.MessageType + message.Message);
            if (message.MessageType == typeof(InVisitorMessage).FullName)
            {
                var msg = Serializer.DeserializeJson<InVisitorMessage>(message.Message);
                Common.Publishers.SafePublisher.send(msg);
            }
            if (message.MessageType == typeof(OutVisitorMessage).FullName)
            {
                var msg = Serializer.DeserializeJson<OutVisitorMessage>(message.Message);
                Common.Publishers.SafePublisher.send(msg);
            }
            if (message.MessageType == typeof(CreateVisitorMessage).FullName)
            {
                var msg = Serializer.DeserializeJson<CreateVisitorMessage>(message.Message);
                Common.Publishers.SafePublisher.send(msg);
            }

            Logs.LogDebug("Deleting message from LiteDb");
            LiteDb.Instance.Dbo
                .GetCollection<LiteDb.Wrapper<BaseMessage>>("failed_messages")
                .DeleteMany(msg => msg.Message == message.Message);
        }
    }
}