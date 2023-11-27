using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using Bogus.DataSets;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.INTRANET.Filters
{
    public class UserAuthorizationAttribute : TypeFilterAttribute
    {
        public UserAuthorizationAttribute() : base(typeof(UserAuthorizationAttributeImp)) { }

        private class UserAuthorizationAttributeImp : IAsyncActionFilter
        {
            private readonly AkdemicContext _context;
            private readonly IConfigurationService _configurationService;
            protected readonly string _userId;
            protected readonly ClaimsPrincipal _principal;

            public UserAuthorizationAttributeImp(
                    AkdemicContext context,
                    IConfigurationService configurationService,
                    IHttpContextAccessor httpContextAccessor,
                    UserManager<ApplicationUser> userManager
                )
            {
                _userId = userManager.GetUserId(httpContextAccessor.HttpContext.User);
                _principal = httpContextAccessor.HttpContext.User;
                _context = context;
                _configurationService = configurationService;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                var requiredFilter = false;

                if (
                    _principal.IsInRole(ConstantHelpers.ROLES.TEACHERS) ||
                    _principal.IsInRole(ConstantHelpers.ROLES.STUDENTS)
                    )
                {
                    if (
                        context.Controller.GetType() != typeof(AKDEMIC.INTRANET.Controllers.JsonController) &&
                        context.Controller.GetType() != typeof(AKDEMIC.INTRANET.Controllers.PasswordChangeController)
                        )
                    {
                        var user = await _context.Users.Where(x => x.Id == _userId).FirstOrDefaultAsync();
                        int.TryParse(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.General.PASSWORD_EXPIRATION_DAYS), out var passwordExpirationDays);

                        if (
                            (user.FirstTime.HasValue && user.FirstTime.Value) ||
                            (!user.PasswordChangeDate.HasValue && passwordExpirationDays != 0) ||
                            (passwordExpirationDays != 0 && user.PasswordChangeDate.HasValue && (System.DateTime.UtcNow - user.PasswordChangeDate.Value).TotalDays > passwordExpirationDays)
                            )
                        {
                            context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "PasswordChange" }, { "action", "Index" } });
                            requiredFilter = true;
                        }
                    }
                }

                if (requiredFilter)
                {
                    var result = next();
                }
                else
                {
                    var result = await next();
                }
            }
        }
    }
}
