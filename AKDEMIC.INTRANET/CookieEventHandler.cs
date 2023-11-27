using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http;
using IdentityModel.Client;

namespace AKDEMIC.INTRANET
{

    public class CookieEventHandler : CookieAuthenticationEvents
    {
        private readonly HttpClient _httpClient;
        private readonly IDiscoveryCache _discoveryCache;

        public CookieEventHandler(
            HttpClient httpClient,
            IDiscoveryCache discoveryCache)
        {
            _httpClient = httpClient;
            _discoveryCache = discoveryCache;
        }

        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            if (context.Principal.Identity.IsAuthenticated)
            {
                //var discovered = await _discoveryCache.GetAsync();
                //var requestResult = await _httpClient.RequestRefreshTokenAsync(new RefreshTokenRequest
                //{
                //    Address = discovered.TokenEndpoint,
                //    ClientId = "intranet",
                //    ClientSecret = "secret"
                //});

                //if (requestResult.IsError)
                //{
                //    var error = $"Error: {requestResult.ErrorDescription}";
                //    //    context.RejectPrincipal();
                //    //    await context.HttpContext.SignOutAsync();
                //}
                //var sub = context.Principal.FindFirst("sub")?.Value;
                //var sid = context.Principal.FindFirst("sid")?.Value;

                //if (LogoutSessions.IsLoggedOut(sub, sid))
                //{
                //    context.RejectPrincipal();
                //    await context.HttpContext.SignOutAsync();

                //    // todo: if we have a refresh token, it should be revoked here.
                //}
            }
        }
    }
}
