using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using AKDEMIC.REPOSITORY.Data;
using static AKDEMIC.CORE.Helpers.ConstantHelpers;

namespace AKDEMIC.INTRANET.Filters
{
    public class RequiresPermissionAttribute : TypeFilterAttribute
    {
        public RequiresPermissionAttribute(params PermissionHelpers.Permission[] permissions) : base(typeof(RequiresPermissionAttributeImpl))
        {
            Arguments = new object[] { permissions };
        }

        private class RequiresPermissionAttributeImpl : IAsyncActionFilter
        {
            protected readonly PermissionHelpers.Permission[] _permissions;
            protected readonly ILogger _logger;
            protected readonly AkdemicContext _dbContext;
            protected readonly string _userId;

            public RequiresPermissionAttributeImpl(ILogger<RequiresPermissionAttribute> logger,
                                                   AkdemicContext dbContext,
                                                   PermissionHelpers.Permission[] permissions,
                                                   IHttpContextAccessor httpContextAccessor)
            {
                _userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                _permissions = permissions;
                _logger = logger;
                _dbContext = dbContext;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context,
                                                     ActionExecutionDelegate next)
            {
                foreach (var permission in _permissions)
                {
                    var p = _dbContext.RolePermission.FirstOrDefault(y => y.PermissionLabel == Enum.GetName(typeof(PermissionHelpers.Permission), permission));

                    if (_dbContext.UserRoles.FirstOrDefault(x => p != null  && x.UserId == _userId) != null)
                    {
                        await next();
                    }
                }

                context.Result = new ChallengeResult();
            }
        }
    }
}
