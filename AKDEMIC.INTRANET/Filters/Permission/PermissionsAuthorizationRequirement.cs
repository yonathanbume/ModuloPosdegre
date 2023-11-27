using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using static AKDEMIC.CORE.Helpers.ConstantHelpers;

namespace AKDEMIC.INTRANET.Filters.Permission
{
    public class PermissionsAuthorizationRequirement : IAuthorizationRequirement
    {
        public IEnumerable<PermissionHelpers.Permission> RequiredPermissions { get; }

        public PermissionsAuthorizationRequirement(IEnumerable<PermissionHelpers.Permission> requiredPermissions)
        {
            RequiredPermissions = requiredPermissions;
        }
    }
}
