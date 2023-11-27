using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AKDEMIC.REPOSITORY.Data
{
    public class AkdemicSeeder<TContext, TUser, TRole>
        where TContext : DbContext
        where TRole : IdentityRole
        where TUser : IdentityUser
    {
        private TContext Context { get; set; }
        private ILogger<AkdemicSeeder<TContext, TUser, TRole>> Logger { get; set; }
        private RoleManager<TRole> RoleManager { get; set; }
        private UserManager<TUser> UserManager { get; set; }

        private void Crash(string message, Exception innerException = null)
        {
            Logger.LogCritical(message);

            throw new ApplicationException($"[AkdemicSeeder] {message}", innerException);
        }

        public AkdemicSeeder(IServiceProvider serviceProvider)
        {
            Logger = serviceProvider.GetRequiredService<ILogger<AkdemicSeeder<TContext, TUser, TRole>>>();

            try
            {
                Context = serviceProvider.GetRequiredService<TContext>();
                RoleManager = serviceProvider.GetRequiredService<RoleManager<TRole>>();
                UserManager = serviceProvider.GetRequiredService<UserManager<TUser>>();
            }
            catch (Exception e)
            {
                Crash("A critical error ocurred", e);
            }
        }

        public void Assemble()
        {
            //Declares the global AKDEMIC.DAL variable and gets all the assemblies for the current domain
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Assembly entityAssembly = null;
            Assembly repositoryAssembly = null;

            //Loops through all the found assemblies and stores the AKDEMIC.DAL assembly
            for (var i = 0; i < assemblies.Length; i++)
            {
                var assembly = assemblies[i];
                var assemblyName = assembly.GetName();
                var assemblyNameName = assemblyName.Name.ToUpper();

                switch (assemblyNameName)
                {
                    case "AKDEMIC.ENTITIES":
                        entityAssembly = assembly;

                        break;
                    case "AKDEMIC.REPOSITORY":
                        repositoryAssembly = assembly;

                        break;
                    default:
                        break;
                }

                if (entityAssembly != null && repositoryAssembly != null)
                {
                    break;
                }
            }

            //Checks if the global assembly exists
            if (repositoryAssembly == null)
            {
                Crash("Repository assembly not found");
            }

            // Models
            var modelTypes = new Dictionary<string, Type>();
            var entityAssemblyTypes = entityAssembly.GetTypes();

            for (var i = 0; i < entityAssemblyTypes.Length; i++)
            {
                var entityAssemblyType = entityAssemblyTypes[i];

                if (
                    entityAssemblyType.IsDefined(typeof(CompilerGeneratedAttribute), false) ||
                    entityAssemblyType.Namespace == null ||
                    entityAssemblyType.Name == "<>c"
                )
                {
                    continue;
                }

                var entityAssemblyTypeNamespace = entityAssemblyType.Namespace.ToUpper();

                if (entityAssemblyTypeNamespace.StartsWith("AKDEMIC.ENTITIES.MODELS"))
                {
                    var entityAssemblyTypeName = entityAssemblyType.Name.ToUpper();

                    modelTypes.Add($"{entityAssemblyTypeNamespace}.{entityAssemblyTypeName}", entityAssemblyType);
                }
            }

            // Seeds
            var seedTypes = new Dictionary<string, Type>();
            var repositoryAssemblyTypes = repositoryAssembly.GetTypes();

            for (var i = 0; i < repositoryAssemblyTypes.Length; i++)
            {
                var repositoryAssemblyType = repositoryAssemblyTypes[i];

                if (
                    repositoryAssemblyType.IsDefined(typeof(CompilerGeneratedAttribute), false) ||
                    repositoryAssemblyType.Namespace == null ||
                    repositoryAssemblyType.Name == "<>c"
                )
                {
                    continue;
                }

                var repositoryAssemblyTypeNamespace = repositoryAssemblyType.Namespace.ToUpper();

                if (repositoryAssemblyTypeNamespace.StartsWith("AKDEMIC.REPOSITORY.DATA.SEEDS"))
                {
                    var repositoryAssemblyTypeName = repositoryAssemblyType.Name.ToUpper();

                    modelTypes.Add($"{repositoryAssemblyTypeNamespace}.{repositoryAssemblyTypeName}", repositoryAssemblyType);
                }
            }
        }
    }
}
