using LiteDB;
using Vis.Common.Models.Messages;

namespace Vis.Client.Database;

public class LiteDb
{
    public static LiteDb Instance { get; } = new();
    public LiteDatabase Dbo { get; }

    public class Wrapper<T>
    {
        public string MessageType = typeof(T).FullName;
        public string Message;
    }
    
    public void Insert<TMessage>(TMessage message)
    {
        var wrappedMessage = new Wrapper<TMessage>
        {
            Message = Common.Models.Serializer.SerializeJson(message)
        };
        Dbo.GetCollection<Wrapper<TMessage>>("failed_messages").Insert(wrappedMessage);
    }

    private LiteDb()
    {
        BsonMapper.Global.IncludeFields = true;
        Dbo = new LiteDatabase("Filename=client.db;connection=shared");
    }
}