using MongoDB.Driver;
using Vis.Common;

namespace Vis.Server.Database;

public class Dbo
{
    private static Dbo? _instance;
    private static MongoClient _client;
    public static Dbo Instance
    {
        get
        {
            if (_instance != null) return _instance;
            Logs.Log(Logs.LogLevel.Debug, "Dbo instance created");
            _instance = new Dbo();
            return _instance;
        }
    }

    public MongoClient Client => _client;
    public IMongoDatabase Database => Client.GetDatabase("vis-data");

    public IMongoCollection<TCollection> GetCollection<TCollection>(string collectionName) =>
        Database.GetCollection<TCollection>(collectionName);
    
    public void InsertOne<TDocument>(string collectionName, TDocument document) {
        Logs.LogDebug("Creating MongoDB session");
        using (var session = Dbo.Instance.Client.StartSession())
        {
            session.StartTransaction();
            Logs.LogDebug($"MongoDB transaction on .{collectionName} with {document?.GetType().Name}");
            
            GetCollection<TDocument>("events").InsertOne(document);
            
            session.CommitTransaction();
            Logs.LogDebug("MongoDB transaction committed successfully");
        }
    }

    private Dbo()
    {
        _client = DbConnect.Connect();
        Logs.Log(Logs.LogLevel.Info, "DBO successfully connected");
    }
    
}