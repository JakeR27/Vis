using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using Vis.Common;
using Vis.Common.Models.Results;
using Vis.Server.Database;

namespace Vis.Server.Endpoints
{
    internal class GetVisitors : BaseGet
    {
        public GetVisitors(string path) : base(path) { }
        protected override IResult callback(HttpContext context)
        {
            Logs.Log(Logs.LogLevel.Info, "Got visitor request");

            var data = new List<Common.Models.Results.VisitorResult>();

            var visitors = Dbo.Instance.Database.GetCollection<Server.Models.Visitor>("people").Find(_ => true).ToList();

            foreach (var visitor in visitors)
            {
                ServerData.visitorsStatus.TryGetValue(visitor.Guid, out var visitorStatus);
                data.Add(new VisitorResult()
                {
                    Visitor = visitor,
                    status = visitorStatus
                });
            }

            return Results.Text(Vis.Common.Models.Serializer.SerializeJson(data));
        }

        
    }
}
