using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Vis.Common;
using Vis.Common.Models.Results;

namespace Vis.Server.Endpoints
{
    internal class GetVisitors : BaseGet
    {
        public GetVisitors(string path) : base(path) { }
        protected override IResult _callback(HttpContext context)
        {
            Logs.Log(Logs.LogLevel.Info, "Got visitor request");

            var data = new List<Common.Models.Results.VisitorResult>();

            foreach (var visitor in ServerData.visitors)
            {
                ServerData.visitorsStatus.TryGetValue(visitor.Key, out var visitorStatus);
                data.Add(new VisitorResult()
                {
                    Visitor = visitor.Value,
                    status = visitorStatus
                });
            }

            return Results.Text(Vis.Common.Models.Serializer.SerializeJson(data));
        }

        
    }
}
