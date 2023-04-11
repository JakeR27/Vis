namespace Vis.Common.Models.Messages
{
    [Serializable]
    public class HostRequestMessage : BaseMessage
    {
        public int OrganisationId;
        public int UnitId;

        public HostRequestMessage() { }
        public HostRequestMessage(int organisationId, int unitId)
        {
            DestinationExchange = Constants.DISCOVERY_XCH;
            RoutingKey = Vis.Common.Constants.HOST_REQUEST_KEY(organisationId, unitId);
            OrganisationId = organisationId;
            UnitId = unitId;
        }
    }
}