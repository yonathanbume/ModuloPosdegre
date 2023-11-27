using System;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AKDEMIC.INTRANET.Filters
{
    public class UserDependencyFilterAttribute : Attribute, IFilterFactory
    {
        public bool IsReusable { get; }

        public string Dependencies { get; set; }

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            return new UserDependencyFilterImpl(
                serviceProvider.GetRequiredService<UserManager<ApplicationUser>>(),
                serviceProvider.GetRequiredService<AkdemicContext>(),
                Dependencies.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries).ToArray());
        }

        private class UserDependencyFilterImpl : IAsyncActionFilter
        {
            private UserManager<ApplicationUser> _userManager { get; set; }
            private AkdemicContext _context { get; set; }
            private string[] _Dependencies { get; set; }

            public UserDependencyFilterImpl(UserManager<ApplicationUser> userManager, AkdemicContext context, string[] Dependencies)
            {
                _userManager = userManager;
                _context = context;
                _Dependencies = Dependencies;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                if (!_Dependencies.Any())
                    throw new ApplicationException("At least one dependency is required.");
                var userId = _userManager.GetUserId(context.HttpContext.User);
                var dependencies = await _context.Dependencies.Where(x => _Dependencies.Contains(x.Name)).ToListAsync();
                var missingDependencies = _Dependencies.Where(x => dependencies.All(y => y.Name != x));
                if (missingDependencies.Any())
                    throw new ApplicationException($"Could not find dependency '${missingDependencies}'.");
                var result =
                    await _context.Users.AnyAsync(x =>
                        x.Id == userId && dependencies.All(d => x.UserDependencies.Any(ud => ud.DependencyId == d.Id)));
                if (!result)
                    context.Result = new ForbidResult();
                else
                    await next();
            }
        }
    }
}
