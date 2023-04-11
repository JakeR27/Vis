using System.Security.Cryptography.X509Certificates;
using MongoDB.Bson;
using MongoDB.Driver;
using Vis.Common;

namespace Vis.Server.Database;

public static class DbConnect
{
    private static string _connectionString = "mongodb+srv://viscluster.1naqtog.mongodb.net/?authSource=%24external&authMechanism=MONGODB-X509&retryWrites=true&w=majority";
    public static MongoClient Connect()
    {
        var settings = _settings();
        Logs.Log(Logs.LogLevel.Debug, "Making DBO connection");
        return new MongoClient(settings);
    }
    
    private static MongoClientSettings _settings()
    {
        var settings = MongoClientSettings.FromConnectionString(_connectionString);
        settings.ServerApi = new ServerApi(ServerApiVersion.V1);
        // You will need to convert your Atlas-provided PEM containing the cert/private keys into a PFX
        // use openssl and the following line to create a PFX from your PEM:
        // openssl pkcs12 -export -in <x509>.pem -inkey <x509>.pem -out <x509>.pfx
        // and provide a password, which should match the second argument you pass to X509Certificate2
        var cert = new X509Certificate2("server-app.pfx", "vis");
        settings.SslSettings = new SslSettings
        {
            ClientCertificates = new List<X509Certificate>(){ cert }
        };
        return settings;
    }
    
    
}