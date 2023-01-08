
using System.Text;
using System.Text.Json;

namespace Vis.Common.Models
{
    public static class Serializer
    {
        static readonly JsonSerializerOptions Options = new() { IncludeFields = true };
        public static byte[] Serialize<T>(T obj)
        {
            var JsonString = System.Text.Json.JsonSerializer.Serialize(obj, Options);
            Logs.Log(Logs.LogLevel.Debug, JsonString);
            return Encoding.UTF8.GetBytes(JsonString);
        }

        public static T Deserialize<T>(byte[] bytes)
        {
            var JsonString = Encoding.UTF8.GetString(bytes);
            Logs.Log(Logs.LogLevel.Debug, JsonString);
            return System.Text.Json.JsonSerializer.Deserialize<T>(JsonString, Options)!;
        }
    }
}
