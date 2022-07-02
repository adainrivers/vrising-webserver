using System;
using System.Collections.Concurrent;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VRising.WebServer.Utils;

namespace VRising.WebServer.Hosting
{
    public class WebHostServer
    {
        private readonly HttpListener _listener;

        public RouteHelper Router { get; }

        public WebHostServer(int port)
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add($"http://*:{port}/");
            Router = new RouteHelper();
        }

        public void Start()
        {
            _listener.Start();
            Receive();
        }

        public void Stop()
        {
            _listener.Stop();
        }

        private void Receive()
        {
            _listener.BeginGetContext(ListenerCallback, _listener);
        }

        private void ListenerCallback(IAsyncResult result)
        {
            if (!_listener.IsListening)
            {
                return;
            }

            var context = _listener.EndGetContext(result);

            if (WorldFactory.Server == null)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "text/plain";
                context.Response.ContentEncoding = Encoding.UTF8;
                context.Response.OutputStream.Write(Encoding.UTF8.GetBytes("Server World is not ready."));
                context.Response.Close();
            }

            var routeAction = Router.GetRouteAction(context);
            if (routeAction != null)
            {
#pragma warning disable CS4014
                // Not awaiting this intentionally
                ProcessRequest(context, routeAction);
#pragma warning restore CS4014
            }
            else
            {
                context.Response.StatusCode = 404;
                context.Response.Close();
            }

            Receive();
        }

        private async Task ProcessRequest(HttpListenerContext context, Func<object> resultFunction)
        {
            var response = context.Response;

            var task = new RisingTask { ResultFunction = resultFunction };
            TaskRunner.TaskQueue.Enqueue(task);
            object result;
            while (!TaskRunner.TaskResults.TryGetValue(task.TaskId, out result))
            {
                await Task.Delay(1);
            }

            response.ContentType = "application/json";
            response.ContentEncoding = Encoding.UTF8;
            var content = JsonConvert.SerializeObject(result, Formatting.Indented);
            response.OutputStream.Write(Encoding.UTF8.GetBytes(content));
            response.Close();
        }

        public class RouteHelper
        {
            private readonly ConcurrentDictionary<string, Func<object>> _routes = new(StringComparer.OrdinalIgnoreCase);

            public void MapGet(string path, Func<object> action)
            {
                _routes.TryAdd("GET " +  path, action);
            }

            internal Func<object> GetRouteAction(HttpListenerContext context)
            {
                var key = context.Request.HttpMethod + " " + context.Request.RawUrl;
                return _routes.TryGetValue(key, out var action) ? action : null;
            }
        }
    }
}
