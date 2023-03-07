using System.Text;

namespace Vis;

public class Constants
{
    public const string DISCOVERY_XCH = Common.Constants.DISCOVERY_XCH;
    
    public static string AUTH_RESPONSE_KEY(int orgId, int unitId) => Common.Constants.AUTH_RESPONSE_KEY(orgId, unitId);
    public static string AUTH_REQUEST_KEY(int orgId, int unitId) => Common.Constants.AUTH_REQUEST_KEY(orgId, unitId);
    public static string HOST_RESPONSE_KEY(int orgId, int unitId) => Common.Constants.HOST_RESPONSE_KEY(orgId, unitId);
    public static string HOST_REQUEST_KEY(int orgId, int unitId) => Common.Constants.HOST_REQUEST_KEY(orgId, unitId);
    public static string BODY_AS_TEXT(dynamic b) => Common.Constants.BODY_AS_TEXT(b);
    
    // new server only constants
    public static string ORGANISATION_XCH(int id) => $"org-{id}-xch";
    public const string BACKEND_XCH = Common.Constants.BACKEND_XCH;

}