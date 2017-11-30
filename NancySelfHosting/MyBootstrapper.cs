namespace NancySelfHosting
{
    using Nancy;
    using Nancy.TinyIoc;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    // Nancy remplaza el bootstraper default por MyBootstrapper
    public class MyBootstrapper : DefaultNancyBootstrapper
    {
        private object _lock = new object();
        private int nextId;
        private List<Stopwatch> _watch; 


        public MyBootstrapper()
        {
            _watch = new List<Stopwatch>();
            nextId = 0;
        }
       
        // Llamado una sola vez cuando el bootstrapper es ejecutado
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);
        }

        protected override void RequestStartup(TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines, NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);

            // hooking up filters
            pipelines.BeforeRequest += CheckSomething;
            pipelines.AfterRequest += ModifyResult;


        }

        private Nancy.Response CheckSomething(Nancy.NancyContext ctx)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            lock (_lock)
            {
                _watch.Add(watch);
                _watch.Insert(nextId, watch);
                ctx.Items.Add("X-watch", nextId);
                nextId++;
            }

            // if you return null, nancy will proceed to module
            return null;
        }

        private void ModifyResult(Nancy.NancyContext ctx)
        {
            var myIndexWatch = Int32.Parse(ctx.Items["X-watch"].ToString());
            lock (_lock)
            {
                var myWatch = _watch[myIndexWatch];
                myWatch.Stop();
                var elapsedMs = myWatch.ElapsedMilliseconds;
                _watch.RemoveAt(myIndexWatch);
                Console.WriteLine(ctx.Request.Method + " " + ctx.Request.Path + ": " + elapsedMs + " miliseconds");
            }
        }
        

    }
}