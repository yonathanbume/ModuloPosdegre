using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Generals;
using Microsoft.EntityFrameworkCore;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;

namespace AKDEMIC.REPOSITORY.Factories
{
    public class ClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>
    {
        private readonly AkdemicContext _context;

        public ClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager, 
            IOptions<IdentityOptions> optionsAccessor,
            AkdemicContext context
        ) 
        : base(userManager, roleManager, optionsAccessor)
        {
            _context = context;
        }

        public override async Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            var principal = await base.CreateAsync(user);
            var identity = (ClaimsIdentity)principal.Identity;
            var roles = await UserManager.GetRolesAsync(user);

            //Putting our Property to Claims

            identity.AddClaims(new[] {
                new Claim(ClaimTypes.UserData, $"{user.FullName}"),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim("PictureUrl", user.Picture ?? ""),
            });

            if (roles.Count > 0)
            {
                var rolePriotiry = await _context.UserRoles
                    .Where(x => x.UserId == user.Id)
                    .OrderByDescending(x => x.Role.Priority)
                    .Select(x => x.Role.Name).FirstOrDefaultAsync();

                identity.AddClaim(new Claim("RolePriorityName", rolePriotiry ));
                identity.AddClaim(new Claim("AkdemicPermissions", string.Join(",", roles)));
            }
            else
            {
                identity.AddClaim(new Claim("RolePriorityName", ""));
            }

            if (principal.IsInRole(ConstantHelpers.ROLES.DEPENDENCY))
            {
                var userDependencies = await _context.UserDependencies
                    .Where(x => x.UserId == user.Id && !x.Dependency.DeletedAt.HasValue)
                    .OrderByDescending(x => x.Dependency.CreatedAt)
                    .Select(x => new
                    {
                        dependencyId = x.Dependency.Id,
                        dependencyName = x.Dependency.Name
                    }).ToListAsync();

                if(userDependencies.Any())
                {
                    var dependencyId = await _context.UserClaims.Where(x => x.UserId == user.Id && x.ClaimType == ConstantHelpers.CLAIMS_USER.DEPENDENCY_ID).FirstOrDefaultAsync();
                    var dependencyName = await _context.UserClaims.Where(x => x.UserId == user.Id && x.ClaimType == ConstantHelpers.CLAIMS_USER.DEPENDENCY_NAME).FirstOrDefaultAsync();

                    if (dependencyId != null &&  !userDependencies.Any(y=>y.dependencyId.ToString() == dependencyId.ClaimValue))
                    {
                        _context.UserClaims.Remove(dependencyId);
                        if (dependencyName != null) _context.UserClaims.Remove(dependencyName);

                        dependencyId = null;
                        dependencyName = null;
                    }

                    if (dependencyId == null)
                        await UserManager.AddClaimAsync(user, new Claim(ConstantHelpers.CLAIMS_USER.DEPENDENCY_ID, $"{userDependencies.Select(y=>y.dependencyId).FirstOrDefault()}"));

                    if (dependencyName == null)
                        await UserManager.AddClaimAsync(user, new Claim(ConstantHelpers.CLAIMS_USER.DEPENDENCY_NAME, $"{userDependencies.Select(y=>y.dependencyName).FirstOrDefault()}"));
               
                }
            }

            return principal;
        }
    }
}
