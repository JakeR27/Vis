using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Vis.Server.Models;

[BsonIgnoreExtraElements]
public class Visitor : Vis.Common.Models.Visitor
{
    public ObjectId Id;
}