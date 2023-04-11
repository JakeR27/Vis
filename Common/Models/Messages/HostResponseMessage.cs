namespace Vis.Common.Models.Messages
{
    [Serializable]
    public class HostResponseMessage : BaseMessage
    {
        public string Host;
        public int Port;

        public HostResponseMessage() { }
        public HostResponseMessage(int organisationId, int unitId)
        {
            DestinationExchange = Constants.DISCOVERY_XCH;
            RoutingKey = Constants.HOST_RESPONSE_KEY(organisationId, unitId);
        }
    }
}