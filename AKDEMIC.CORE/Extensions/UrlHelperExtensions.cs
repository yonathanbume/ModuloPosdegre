using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.CORE.Extensions
{
    public static class UrlHelperExtensions
    {
        public static string GenerateLink(this IUrlHelper urlHelper, string action, string controller, string scheme, object args)
        {
            return urlHelper.Action(
                action: action,
                controller: controller,
                values: args,
                protocol: scheme);
        }

        public static string GenerateLink(this IUrlHelper urlHelper, string action, string controller)
        {
            return urlHelper.Action(
                action: action,
                controller: controller);
        }
    }
}
