namespace NancySelfHosting
{
    using Nancy;
    using Nancy.TinyIoc;

    // Nancy remplaza el bootstraper default por MyBootstrapper
    public class MyBootstrapper : DefaultNancyBootstrapper
    {
        public MyBootstrapper()
        {
        }
  
        // Llamado una sola vez cuando el bootstrapper es ejecutado
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);
        }
    }   
}