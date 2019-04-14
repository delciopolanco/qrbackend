using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace qr_backend
{
    public class Program {
        public static void Main (string[] args) {
            CreateWebHostBuilder (args).Build ().Run ();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .UseKestrel()
            //.UseUrls("http://172.20.234.53:5000")
            .UseUrls("http://172.20.235.166:8080")
            .UseIISIntegration()
            .ConfigureKestrel((context, options) =>
            {
                // Set properties and call methods on options
            });

    //    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
    //WebHost.CreateDefaultBuilder(args)
    //    .UseStartup<Startup>();
    }
}