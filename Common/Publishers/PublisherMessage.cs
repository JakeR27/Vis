namespace Vis.Common
{
    internal partial class Publishers
    {
        [Serializable]
        class PublisherMessage
        {
            public string exchange;
            public string routingKey;
            public byte[] message;

            private static long messageId;

            private PublisherMessage()
            {

            }
            public PublisherMessage(string exchange, string routingKey, byte[] message)
            {
                this.exchange = exchange;
                this.routingKey = routingKey;
                this.message = message;
            }

            public byte[] Serialize()
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (BinaryWriter bw = new BinaryWriter(ms))
                    {
                        bw.Write(exchange);
                        bw.Write(routingKey);
                        bw.Write(message.Length);
                        bw.Write(message);
                    }
                    return ms.ToArray();
                }
            }

            public static PublisherMessage Deserialize(byte[] data)
            {
                PublisherMessage result = new PublisherMessage();
                using (MemoryStream m = new MemoryStream(data)) {
                    using (BinaryReader reader = new BinaryReader(m)) {
                        result.exchange = reader.ReadString();
                        result.routingKey = reader.ReadString();
                        var messageLength = reader.ReadInt32();
                        result.message = reader.ReadBytes(messageLength);
                    }
                }
                return result;
            }
        }
    }
    
}
