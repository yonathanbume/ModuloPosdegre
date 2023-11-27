using System;
using System.IO;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.REPOSITORY.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AKDEMIC.INTRANET
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                 .AddJsonFile("settings.json", optional: false, reloadOnChange: true)
                 .Build();

            var buildWebHost = CreateWebHostBuilder(args, configuration).Build();
            //var buildWebHost = BuildWebHost(args).Run();

            if (ConstantHelpers.Seeder.Enabled)
            {
                using (IServiceScope scope = buildWebHost.Services.CreateScope())
                {
                    IServiceProvider serviceProvider = scope.ServiceProvider;

                    try
                    {
                        AkdemicContext context = serviceProvider.GetRequiredService<AkdemicContext>();
                        RoleManager<ApplicationRole> roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
                        UserManager<ApplicationUser> userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                        ILogger<DbInitializer> logger = serviceProvider.GetRequiredService<ILogger<DbInitializer>>();

                        context.Database.EnsureCreated();
                        DbInitializer.Initializer(context, userManager, roleManager, logger).Wait();
                    }
                    catch (Exception e)
                    {
                        ILogger<Program> logger = serviceProvider.GetRequiredService<ILogger<Program>>();

                        logger.LogCritical(e, "An error occurred while seeding the database.");
                        throw new Exception("An error occurred while seeding the database.", e);
                        //Environment.Exit(0);
                    }
                }
            }

            buildWebHost.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args, IConfiguration config) =>
            WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(config)
                .UseStartup<Startup>();
    }
}
