using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

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
        protected abstract IResult _callback(HttpContext context);
    }
}
