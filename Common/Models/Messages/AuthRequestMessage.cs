namespace Vis.Common.Models.Messages
{
    [Serializable]
    public class AuthRequestMessage : BaseMessage
    {
        public int OrganisationId;
        public int UnitId;
        public string Secret;

        public AuthRequestMessage() { }
        public AuthRequestMessage(int organisationId, int unitId, string secret)
        {
            DestinationExchange = Constants.DISCOVERY_XCH;
            RoutingKey = Vis.Common.Constants.AUTH_REQUEST_KEY(organisationId, unitId);
            Secret = secret;
            OrganisationId = organisationId;
            UnitId = unitId;
        }
    }
}