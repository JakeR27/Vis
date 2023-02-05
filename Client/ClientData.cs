﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vis.Common.Models;

namespace Vis.Client
{
    internal static class ClientData
    {
        public static int _organisationId = 10;
        public static int _unitId = 1;
        public static bool _organisationExchangeFound = false;
        public static string _organisationExchangeName = string.Empty;

        public static bool _serverHostFound = false;

        //temp
        public static Dictionary<Guid, Visitor> visitors = new();
        public static Dictionary<Guid, bool> visitorsStatus = new();

        //very temp
        public static void displayVisitors()
        {
            Console.Clear();
            int i = 0;
            foreach (var visitorEntry in ClientData.visitors)
            {
                ClientData.visitorsStatus.TryGetValue(visitorEntry.Key, out var status);

                Console.WriteLine($"{i}. {visitorEntry.Value.Guid.ToString("N")[..5]} - {(status ? "IN" : "OUT")} - {visitorEntry.Value.Name}");
                i++;
            }
        }

    }
}
