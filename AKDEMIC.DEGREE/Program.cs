using System;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MoreLinq.Extensions;

namespace AKDEMIC.DEGREE
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //BuildWebHost(args).Run();

            var buildWebHost = CreateWebHostBuilder(args).Build();
            var pivot_generate = ConstantHelpers.Seeder.Enabled;
            //variable desactivado para no generarar datos de pueba y usuarios nuevamente
            pivot_generate = true;

            if (pivot_generate)
            {
                using (var scope = buildWebHost.Services.CreateScope())
                {
                    var serviceProvider = scope.ServiceProvider;

                    try
                    {
                        var context = serviceProvider.GetRequiredService<AkdemicContext>();
                        var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
                        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                        var logger = serviceProvider.GetRequiredService<ILogger<DbInitializer>>();

                        context.Database.EnsureCreated();
                        DbInitializer.Initializer(context, userManager, roleManager, logger).Wait();

                        //var seeder = new Seeder<AkdemicContext, ApplicationUser, ApplicationRole, DbInitializer>(context, domainAssemblyName, assemblyTypeNamespace, modelPriorities, modelExclusions, userManager, roleManager, logger);
                        //seeder.Run().Wait();
                    }
                    catch (Exception e)
                    {
                        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

                        logger.LogCritical(e, "An error occurred while seeding the database.");
                        throw new Exception("An error occurred while seeding the database.", e);
                        //Environment.Exit(0);
                    }
                }
            }

            buildWebHost.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        //    public static IWebHost BuildWebHost(string[] args) =>
        //WebHost.CreateDefaultBuilder(args)
        //    .UseStartup<Startup>()
        //    .Build();
    }
}
