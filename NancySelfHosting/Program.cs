namespace NancySelfHosting
{
    using System;
    using System.Diagnostics;
    using Nancy.Hosting.Self;

    class Program
    {
        static void Main()
        {
            // Reserva la url para poder levantar el NancyHost. Desde Visual Studio requiere ser ejecutado como admin
            HostConfiguration hostConfigs = new HostConfiguration();
            hostConfigs.UrlReservations.CreateAutomatically = true;
            hostConfigs.RewriteLocalhost = false;
            String hostingUrl = "http://localhost:50427";

            NancyHost nancyHost = new NancyHost(new Uri(hostingUrl), new MyBootstrapper(), hostConfigs);

            using (nancyHost)
            {
                nancyHost.Start();

                Console.WriteLine("Nancy now listening in "+ hostingUrl +". Press enter to stop");
                try
                {
                    Process.Start(hostingUrl);
                }
                catch (Exception)
                {
                }
                Console.ReadKey();
            }

            Console.WriteLine("Stopped. Good bye!");
        }
    }
}