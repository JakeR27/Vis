using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Vis.Server.Models;

[BsonIgnoreExtraElements]
public class OrganisationSecret : Vis.Common.Models.OrganisationSecret
{
    public ObjectId Id;
}