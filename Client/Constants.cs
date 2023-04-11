namespace Vis;

public class Constants
{
    public const string DISCOVERY_XCH = Common.Constants.DISCOVERY_XCH;
    public static string AUTH_RESPONSE_KEY(int orgId, int unitId) => Common.Constants.AUTH_RESPONSE_KEY(orgId, unitId);
    public static string HOST_RESPONSE_KEY(int orgId, int unitId) => Common.Constants.HOST_RESPONSE_KEY(orgId, unitId);
}