namespace Vis.WebServer
{
    public class App
    {
        private static WebApplicationBuilder _builder = WebApplication.CreateBuilder();
        public static WebApplication WebApp = _builder.Build();
    }
}