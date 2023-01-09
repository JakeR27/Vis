namespace Vis.Common.Models.Messages
{
    [Serializable]
    public class AuthResponseMessage : BaseMessage
    {
        public bool Success = false;
        public string OrganisationExchangeName;

        public AuthResponseMessage() { }
        public AuthResponseMessage(int organisationId, int unitId)
        {
            DestinationExchange = Constants.DISCOVERY_XCH;
            RoutingKey = Constants.AUTH_RESPONSE_KEY(organisationId, unitId);
        }
    }
}