using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc.Infrastructure;
using Microsoft.AspNet.Mvc.Routing;
using Microsoft.AspNet.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomRoutingAndRedirectProblem.Framework
{
    public class MyRouterMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;
        private IRouter _router;
        private readonly IApplicationBuilder _app;
        private readonly Action<IRouteBuilder> _configureRoutes;

        public static MyRouterMiddleware Current { get; set; }

        public MyRouterMiddleware(
            RequestDelegate next,
            ILoggerFactory loggerFactory,
            IApplicationBuilder app, Action<IRouteBuilder> configureRoutes)
        {
            _next = next;
            _app = app;

            _logger = loggerFactory.CreateLogger<RouterMiddleware>();
            _configureRoutes = configureRoutes;

            Current = this;
            ReloadRoutes();
        }

        public void ReloadRoutes()
        {
            var routes = new RouteBuilder()
            {
                DefaultHandler = new MvcRouteHandler(),
                ServiceProvider = _app.ApplicationServices
            };

            _configureRoutes(routes);

            // Adding the attribute route comes after running the user-code because
            // we want to respect any changes to the DefaultHandler.
            routes.Routes.Insert(0, AttributeRouting.CreateAttributeMegaRoute(
                routes.DefaultHandler,
                _app.ApplicationServices));

            _router = routes.Build();
        }


        public async Task Invoke(HttpContext httpContext)
        {
            var context = new RouteContext(httpContext);
            context.RouteData.Routers.Add(_router);
            await _router.RouteAsync(context);

            if (!context.IsHandled)
            {
                await _next.Invoke(httpContext);
            }
            else
            {
                httpContext.Features[typeof(IRoutingFeature)] = new RoutingFeature()
                {
                    RouteData = context.RouteData,
                };
                await _next.Invoke(context.HttpContext);
            }
        }

        private class RoutingFeature : IRoutingFeature
        {
            public RouteData RouteData { get; set; }
        }
    }

    public interface IRoutingFeature
    {
        /// <summary>
        /// Gets or sets the <see cref="Routing.RouteData"/> associated with the current request.
        /// </summary>
        RouteData RouteData { get; set; }
    }
}
