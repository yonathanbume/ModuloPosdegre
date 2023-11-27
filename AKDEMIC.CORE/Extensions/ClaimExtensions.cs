using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using static AKDEMIC.CORE.Helpers.ConstantHelpers;

namespace AKDEMIC.CORE.Extensions
{
    public static class ClaimExtensions
    {
        public static string GetEmail(this ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                return user.Claims.FirstOrDefault(v => v.Type == ClaimTypes.Email).Value;
            }

            return "";
        }

        public static string GetFullName(this ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                List<Claim> a = user.Claims.ToList();
                return user.Claims.FirstOrDefault(v => v.Type == ClaimTypes.UserData).Value;
            }

            return "";
        }

        public static string GetPhone(this ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                return user.Claims.FirstOrDefault(v => v.Type == ClaimTypes.HomePhone).Value;
            }

            return "";
        }

        public static string GetPictureUrl(this ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                return user.Claims.FirstOrDefault(v => v.Type == "PictureUrl").Value;
            }

            return "";
        }

        public static string GetRoles(this ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                //return user.Claims.FirstOrDefault(v => v.Type == ClaimTypes.Role).Value;
                return user.Claims.FirstOrDefault(v => v.Type == "RolePriorityName").Value;
            }

            return "";
        }

        public static bool HasPermission(this ClaimsPrincipal principal, PermissionHelpers.Permission role)
        {
            Claim _claim = principal.Claims.FirstOrDefault(x => x.Type == "AkdemicPermissions");

            string roleLabel = Enum.GetName(typeof(PermissionHelpers.Permission), role);

            string[] _roles = _claim.ToString().Split(',');

            foreach (string _r in _roles)
            {
                if (_r == roleLabel)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool HasPermission(this ClaimsPrincipal principal, PermissionHelpers.Permission[] roles)
        {

            Claim _claim = principal.Claims.FirstOrDefault(x => x.Type == "AkdemicPermissions");

            string[] _roles = _claim.ToString().Split(',');

            foreach (PermissionHelpers.Permission r in roles)
            {
                string roleLabel = Enum.GetName(typeof(PermissionHelpers.Permission), r);

                foreach (string _r in _roles)
                {
                    if (_r == roleLabel)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
