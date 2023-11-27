using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Generals;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AKDEMIC.REPOSITORY.Data
{
    public class DbInitializer
    {
        public static async Task Initializer(AkdemicContext apiContext, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ILogger<DbInitializer> logger)
        {
            //Declares the global AKDEMIC.DAL variable and gets all the assemblies for the current domain
            Assembly entitiesAssembly = null;
            Assembly repositoryAssembly = null;

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            //Loops through all the found assemblies and stores the AKDEMIC.DAL assembly
            foreach (Assembly assembly in assemblies)
            {
                AssemblyName assemblyName = assembly.GetName();

                if (assemblyName.Name == "AKDEMIC.ENTITIES")
                {
                    entitiesAssembly = assembly;
                }
                else if (assemblyName.Name == "AKDEMIC.REPOSITORY")
                {
                    repositoryAssembly = assembly;
                }

                if (entitiesAssembly != null && repositoryAssembly != null)
                {
                    break;
                }
            }

            //Checks if the global assembly exists
            if (entitiesAssembly == null || repositoryAssembly == null)
            {
                ApplicationException exception = new ApplicationException($"Assembly not found");
                throw exception;
            }

            //Declares the dictionaries to store the types of the AKDEMIC.DAL.Models and AKDEMIC.DAL.Seeds namespaces and gets all the types from the global assembly
            Dictionary<string, Type> modelsTypes = new Dictionary<string, Type>();
            Dictionary<string, Type> seedsTypes = new Dictionary<string, Type>();

            Type[] entitiesAssemblyTypes = entitiesAssembly.GetTypes();
            Type[] repositoryAssemblyTypes = repositoryAssembly.GetTypes();

            //Loops through all the assembly types and stores the type if the namespace is either AKDEMIC.DAL.Models or AKDEMIC.DAL.Seeds

            foreach (Type entitiesAssemblyType in entitiesAssemblyTypes)
            {
                if (
                    entitiesAssemblyType.IsDefined(typeof(CompilerGeneratedAttribute), false) ||
                    entitiesAssemblyType.Namespace == null ||
                    entitiesAssemblyType.Name == "<>c"
                )
                {
                    continue;
                }

                if (entitiesAssemblyType.Namespace.StartsWith("AKDEMIC.ENTITIES.Models"))
                {
                    // In any case, ProjectSeed wont work

                    if (modelsTypes.ContainsKey(entitiesAssemblyType.Name))
                    {
                        continue;
                    }

                    modelsTypes.Add(entitiesAssemblyType.Name, entitiesAssemblyType);
                }
            }

            foreach (Type repositoryAssemblyType in repositoryAssemblyTypes)
            {
                if (
                    repositoryAssemblyType.IsDefined(typeof(CompilerGeneratedAttribute), false) ||
                    repositoryAssemblyType.Namespace == null ||
                    repositoryAssemblyType.Name == "<>c"
                )
                {
                    continue;
                }

                if (repositoryAssemblyType.Namespace.StartsWith("AKDEMIC.REPOSITORY.Data.Seeds"))
                {
                    seedsTypes.Add(repositoryAssemblyType.Name, repositoryAssemblyType);
                }
            }

            //Declares the dictionaries to store the apiContext properties and gets all the properties from the apiContext type
            Dictionary<string, PropertyInfo> apiContextProperties = new Dictionary<string, PropertyInfo>();
            Type apiContextType = apiContext.GetType();
            PropertyInfo[] apiContextTypeProperties = apiContextType.GetProperties();

            //Loops through all the type properties and stores the type property if it has the dbSet type
            foreach (PropertyInfo apiContextTypeProperty in apiContextTypeProperties)
            {
                //Gets the apiContext property type name
                Type apiContextTypePropertyType = apiContextTypeProperty.PropertyType;
                string apiContextTypePropertyTypeName = apiContextTypePropertyType.Name;
                string[] apiContextTypePropertyTypeNameSplit = apiContextTypePropertyTypeName.Split("`");

                //Gets the dbSet type name
                Type dbSetType = typeof(DbSet<>);
                string dbSetTypeName = dbSetType.Name;
                string[] dbSetTypeNameSplit = dbSetTypeName.Split("`");

                //Checks if the apiContext property type name and the dbSet type name are the same
                if (apiContextTypePropertyTypeNameSplit[0] == dbSetTypeNameSplit[0])
                {
                    Type[] apiContextTypePropertyTypeGenericArguments = apiContextTypePropertyType.GetGenericArguments();

                    if (apiContextProperties.ContainsKey(apiContextTypePropertyTypeGenericArguments[0].Name))
                    {
                        continue;
                    }

                    apiContextProperties.Add(apiContextTypePropertyTypeGenericArguments[0].Name, apiContextTypeProperty);
                }
            }

            List<string> seedPriorities = ConstantHelpers.Seeder.Priorities;

            //Loops though all the seeds declared in the seedPriorities constant
            foreach (string seedPriority in seedPriorities)
            {
                //Gets the current seed type and invokes its Seed method
                string seedTypeKey = seedPriority;

                if (!seedsTypes.TryGetValue(seedTypeKey, out Type seedTypeValue))
                {
                    continue;
                }

                await Seed(apiContext, userManager, roleManager, logger, seedTypeKey, seedTypeValue, modelsTypes, apiContextProperties);

                seedsTypes.Remove(seedTypeKey);
            }

            //Loops through the remaining seeds in the seedsTypes dictionary
            foreach (KeyValuePair<string, Type> seedType in seedsTypes)
            {
                string seedTypeKey = seedType.Key;
                Type seedTypeValue = seedType.Value;

                await Seed(apiContext, userManager, roleManager, logger, seedTypeKey, seedTypeValue, modelsTypes, apiContextProperties);
            }
        }

        public static async Task Seed(AkdemicContext apiContext, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ILogger<DbInitializer> logger, string seedTypeKey, Type seedTypeValue, Dictionary<string, Type> modelsTypes, Dictionary<string, PropertyInfo> apiContextProperties)
        {
            object seedList;

            try
            {
                //Gets the current seed type and invokes its Seed method
                MethodInfo seedMethod = seedTypeValue.GetMethod("Seed");
                seedList = seedMethod.Invoke(null, new object[] { apiContext });
            }
            catch (Exception e)
            {
                ApplicationException exception = new ApplicationException($"Could not invoke Seed method from '{seedTypeKey}'", e.InnerException);
                throw exception;
            }

            try
            {
                if (seedTypeKey == ConstantHelpers.Seeder.IdentityRole)
                {
                    //Adds a new role
                    await SeedHandler(apiContext, roleManager, logger, seedTypeValue, seedList);
                }
                else if (seedTypeKey == ConstantHelpers.Seeder.UserRole)
                {
                    //Adds a new user role
                    await SeedHandler(apiContext, userManager, logger, seedTypeValue, seedList);
                }
                else
                {
                    //Adds a new entity
                    await SeedHandler(apiContext, logger, seedList, seedTypeKey, modelsTypes, apiContextProperties);
                }
            }
            catch (Exception e)
            {
                ApplicationException exception = new ApplicationException($"Could not add data from '{seedTypeKey}'", e);
                throw exception;
            }

            try
            {
                //Saves the current changes on the context
                int apiContextChanges = await apiContext.SaveChangesAsync();
                logger.LogDebug($"{apiContextChanges} {(apiContextChanges != 1 ? "changes" : "change")} saved successfully.");
            }
            catch (Exception e)
            {
                ApplicationException exception = new ApplicationException($"Could not save changes from '{seedTypeKey}'", e);
                throw exception;
            }
        }

        public static async Task SeedHandler(AkdemicContext apiContext, ILogger<DbInitializer> logger, object seedList, string seedName, Dictionary<string, Type> modelsTypes, Dictionary<string, PropertyInfo> apiContextProperties)
        {
            //Gets the model for the current seed
            string modelTypeKey = seedName.Replace("Seed", "");
            Type modelTypeValue = modelsTypes[modelTypeKey];

            //Gets the apiContext property value object for the current seed
            PropertyInfo apiContextPropertyValue = apiContextProperties[modelTypeKey];
            object apiContextPropertyValueInfo = apiContextPropertyValue.GetValue(apiContext);

            //Invokes the SeedData method for the current seed
            MethodInfo seedDataMethod = typeof(DbInitializer).GetMethod("SeedData");
            MethodInfo seedDataGenericMethod = seedDataMethod.MakeGenericMethod(modelTypeValue);

            await (dynamic)seedDataGenericMethod.Invoke(null, new object[] { apiContextPropertyValueInfo, seedList });
        }

        public static async Task SeedHandler(AkdemicContext apiContext, RoleManager<ApplicationRole> roleManager, ILogger<DbInitializer> logger, Type seedType, object seedList)
        {
            MethodInfo seedMethod = seedType.GetMethod("Seed");
            seedMethod.Invoke(null, new object[] { apiContext });
            ApplicationRole[] roles = (ApplicationRole[])seedList;

            await SeedRoleData(roleManager, apiContext.Roles, roles, logger);
        }

        public static async Task SeedHandler(AkdemicContext apiContext, UserManager<ApplicationUser> userManager, ILogger<DbInitializer> logger, Type seedType, object seedList)
        {
            MethodInfo seedMethod = seedType.GetMethod("Seed");
            seedMethod.Invoke(null, new object[] { apiContext });
            Tuple<ApplicationUser, string[], string>[] usersRoles = (Tuple<ApplicationUser, string[], string>[])seedList;

            await SeedRoleUserData(userManager, apiContext.UserRoles, usersRoles, logger);
        }

        public static async Task SeedData<T>(DbSet<T> dbSet, T[] entities) where T : class
        {
            if (dbSet.Any())
            {
                return;
            }

            await dbSet.AddRangeAsync(entities);
        }

        public static async Task SeedRoleData(RoleManager<ApplicationRole> roleManager, DbSet<ApplicationRole> dbSet, ApplicationRole[] roles, ILogger<DbInitializer> logger)
        {
            if (dbSet.Any())
            {
                return;
            }

            foreach (ApplicationRole role in roles)
            {
                IdentityResult identityResult = await roleManager.CreateAsync(role);

                if (identityResult.Succeeded)
                {
                    logger.LogDebug($"Added role '{role}'");
                }
                else
                {
                    ApplicationException exception = new ApplicationException($"Could not add role '{role}'");
                    throw exception;
                }
            }
        }

        public static async Task SeedRoleUserData(UserManager<ApplicationUser> userManager, DbSet<ApplicationUserRole> dbSet, Tuple<ApplicationUser, string[], string>[] usersRoles, ILogger<DbInitializer> logger)
        {
            if (dbSet.Any())
            {
                return;
            }

            foreach (Tuple<ApplicationUser, string[], string> userRoles in usersRoles)
            {
                ApplicationUser user = userRoles.Item1;
                string[] roles = userRoles.Item2;
                string password = userRoles.Item3;

                IdentityResult identityResult = await userManager.CreateAsync(user);

                if (identityResult.Succeeded)
                {
                    logger.LogDebug($"Added user '{user}'");

                    identityResult = await userManager.AddPasswordAsync(user, password);

                    if (identityResult.Succeeded)
                    {
                        logger.LogDebug($"Added password to user '{user}'");
                    }
                    else
                    {
                        ApplicationException exception = new ApplicationException($"Could not add password to user '{user}'");
                        throw exception;
                    }

                    identityResult = await userManager.AddToRolesAsync(user, roles);

                    if (identityResult.Succeeded)
                    {
                        logger.LogDebug($"Added '{user}' to roles '{roles}'");
                    }
                    else
                    {
                        ApplicationException exception = new ApplicationException($"Could not add user '{user}' to roles '{roles}'");
                        throw exception;
                    }
                }
                else
                {
                    ApplicationException exception = new ApplicationException($"Could not add user '{user}'");
                    throw exception;
                }
            }
        }
    }
}
