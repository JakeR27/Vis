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
using Vis.Server.Startup;

namespace Vis.Server.Endpoints
{
    internal class GetVisitors : BaseGet
    {
        public GetVisitors(string path) : base(path) { }
        protected override IResult callback(HttpContext context)
        {
            
            var data = new List<Common.Models.Results.VisitorResult>();
            int organisationId;
            try
            {
                var orgTemp = context.Request.Query["organisationid"].ToString();
                organisationId = int.Parse(orgTemp);
                Logs.Log(Logs.LogLevel.Info, $"Got visitor request for org: {organisationId}");
            }
            catch
            {
                Logs.LogWarning("Failed to parse organisation ID while handling visitor request");
                return Results.Text(Common.Models.Serializer.SerializeJson(data));
            }
            
            var visitors = Dbo.Instance
                .GetCollection<Server.Models.Visitor>("people")
                .Find(visitor => visitor.OrganisationId == organisationId)
                .ToList();
            new VisitorStateParser().Execute();

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
