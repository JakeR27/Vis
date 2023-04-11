using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Vis.Common;

namespace Vis.Server.Endpoints
{
    internal abstract class BaseGet
    {
        protected string _path;

        public BaseGet(string path)
        {
            _path = path;
        }
        public void handle()
        {
            Vis.WebServer.App.WebApp.MapGet(_path, _callback);
        }

        private IResult _callback(HttpContext context)
        {
            Logs.LogDebug($"Received HTTP GET request on {_path}");
            return callback(context);
        }

        protected abstract IResult callback(HttpContext context);
    }
}
