using Nancy;
using System;

namespace NancySelfHosting.Modules
{
    public class RouteConfigModule : NancyModule
    {
        public RouteConfigModule() 
        {
            Get("/", args => "Hello World! This is a Nancy Self Hosting Server");

            Get("/{whatever}", (_) =>
            {
                return HttpStatusCode.MethodNotAllowed;
            });

            Post("/{whatever}", (_) =>
            {
                return HttpStatusCode.MethodNotAllowed;
            });

            Delete("/{whatever}", (_) =>
            {
                return HttpStatusCode.MethodNotAllowed;
            });

            Patch("/{whatever}", (_) =>
            {
                return HttpStatusCode.MethodNotAllowed;
            });

        }
    }
}