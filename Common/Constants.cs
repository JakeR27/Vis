﻿using System.Text;

namespace Vis.Common
{
    public static class Constants
    {
        public const string DISCOVERY_XCH = "discovery-xch";
        public static string AUTH_RESPONSE_KEY(int orgId, int unitId) => $"{orgId}.{unitId}.responses.auth";
        public static string AUTH_REQUEST_KEY(int orgId, int unitId) => $"{orgId}.{unitId}.requests.auth";
        public static string HOST_RESPONSE_KEY(int orgId, int unitId) => $"{orgId}.{unitId}.responses.host";
        public static string HOST_REQUEST_KEY(int orgId, int unitId) => $"{orgId}.{unitId}.requests.host";
        public static string BODY_AS_TEXT(dynamic b) => Encoding.UTF8.GetString(b.ToArray());
    }
}