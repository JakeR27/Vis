using System.Collections.Immutable;
using MongoDB.Bson;
using MongoDB.Driver;
using Vis.Common;
using Vis.Common.Startup;
using Vis.Server.Database;
using Vis.Server.Models;

namespace Vis.Server.Startup;

public class VisitorStateParser : BaseStartupTask
{
    protected override string _taskDescription => "Parses visitor event log to determine visitor state";

    protected override void _execute()
    {
        //gets the most recent event for each visitor
        var t = Dbo.Instance.GetCollection<VisitorEvent>("events")
            .AsQueryable()
            .OrderByDescending(ve=>ve.Timestamp)
            .GroupBy(ve=>ve.VisitorId)
            .Select(x=>x.First());
        
        
        foreach (var e in t.ToList())
        {
            Logs.LogDebug($"{e.Timestamp} {e.VisitorId} - {e.EventType}");
            ServerData.visitorsStatus[e.VisitorId] = e.EventType;
        }

        throw new NotImplementedException();
    }
}