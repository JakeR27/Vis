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

    private Dbo()
    {
        _client = DbConnect.Connect();
        Logs.Log(Logs.LogLevel.Info, "DBO successfully connected");
    }
    
}