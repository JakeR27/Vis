using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vis.Common.Models;
using Vis.Server.Models;
using Visitor = Vis.Common.Models.Visitor;

namespace Vis.Server
{
    internal static class ServerData
    {
        //temp
        public static Dictionary<Guid, Visitor> visitors = new();
        public static Dictionary<Guid, VisitorEventEnum> visitorsStatus = new();
    }
}
