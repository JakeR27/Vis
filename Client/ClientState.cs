﻿using Vis.Common;
using Vis.Common.Models;

namespace Vis.Client
{
    public static class ClientState
    {
        public static int _organisationId = 10;
        public static int _unitId = 1;
        public static string _organisationExchangeName = string.Empty;
        public static string _organisationSecret = string.Empty;

        public static bool _serverHostFound = false;

        //temp
        public static Dictionary<Guid, Visitor> visitors = new();
        public static Dictionary<Guid, VisitorEventEnum> visitorsStatus = new();

        //very temp
        [Obsolete]
        public static void displayVisitors()
        {
            Console.Clear();
            int i = 0;
            foreach (var visitorEntry in ClientState.visitors)
            {
                ClientState.visitorsStatus.TryGetValue(visitorEntry.Key, out var status);

                Console.WriteLine($"{i}. {visitorEntry.Value.Guid.ToString("N")[..5]} - {status} - {visitorEntry.Value.Name}");
                i++;
            }
        }

    }
}
