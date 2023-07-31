using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.IO;

namespace IdentityApi
{
    public class Program
    {
        public static readonly string Namespace = typeof(Program).Namespace;
        public static readonly string AppName = Namespace[(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1)..];
        //public static readonly string AppName = Namespace.Substring(startIndex: Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);

        public static int Main(string[] args)
        {
            var configuration = GetConfiguration();

            // Log.Logger = CreateSerilogLogger(configuration);

            try
            {
                //  Log.Information("Configuring web host ({ApplicationContext})...", AppName);
                var host = BuildWebHost(configuration, args);

                // Log.Information("Applying migrations ({ApplicationContext})...", AppName);
                host.MigrateDbContext<PersistedGrantDbContext>((_, __) => { })
                   .MigrateDbContext<ApplicationDbContext>((applicationDbContext, services) =>
                   {
                       var env = services.GetService<IWebHostEnvironment>();
                       var userManager = services.GetService<UserManager<ApplicationUser>>();
                       var logger = services.GetService<ILogger<ApplicationDbContextSeed>>();

                       new ApplicationDbContextSeed()
                           .SeedAsync(applicationDbContext, env, userManager, logger)
                           .Wait();
                   })
                   .MigrateDbContext<ConfigurationDbContext>((configurationDbContext, services) =>
                   {
                       new ConfigurationDbContextSeed()
                           .SeedAsync(configurationDbContext)
                           .Wait();
                   });


                //Log.Information("Starting web host ({ApplicationContext})...", AppName);
                host.Run();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", AppName);
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
        private static IWebHost BuildWebHost(IConfiguration configuration, string[] args) =>
           WebHost.CreateDefaultBuilder(args)
               .CaptureStartupErrors(false)
               .ConfigureAppConfiguration(x => x.AddConfiguration(configuration))
               .UseStartup<Startup>()
               .UseContentRoot(Directory.GetCurrentDirectory())

               .Build();


        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                //  .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            //var config = builder.Build();
            return builder.Build();
        }
    }
}
