using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Vis.Server.Models;

[BsonIgnoreExtraElements]
public class VisitorEvent
{
    public ObjectId Id;
    public Guid VisitorId;
    public VisitorEventEnum EventType;
    public DateTime Timestamp;
}