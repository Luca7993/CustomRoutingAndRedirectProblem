using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Mvc.Infrastructure;
using Microsoft.AspNet.Mvc.Routing;
using Microsoft.AspNet.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomRoutingAndRedirectProblem.Framework
{
    public static class MyRoutingBuilderExtensions
    {
        public static IApplicationBuilder UseMyRouterMiddleware(this IApplicationBuilder app, Action<IRouteBuilder> configureRoutes)
        {
            var routes = new RouteBuilder()
            {
                ServiceProvider = app.ApplicationServices,
                DefaultHandler = new MvcRouteHandler()
            };


            // Adding the attribute route comes after running the user-code because
            // we want to respect any changes to the DefaultHandler.
            routes.Routes.Insert(0, AttributeRouting.CreateAttributeMegaRoute(
                routes.DefaultHandler,
                app.ApplicationServices));
            
            
             
            app.UseMiddleware<MyRouterMiddleware>(app, configureRoutes);
            return app.UseMiddleware<RouterMiddleware>(routes.Build()); 
        }
    }
}
