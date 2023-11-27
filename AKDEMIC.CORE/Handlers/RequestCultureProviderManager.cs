using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

namespace AKDEMIC.CORE.Handlers
{
    public class RequestCultureProviderManager : IRequestCultureProvider
    {
        public Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            try
            {
                var userLanguages = httpContext.Request.GetTypedHeaders().AcceptLanguage;
                var language = ConstantHelpers.Language.Spanish;

                var supportedCultures = new List<string>
                {
                    ConstantHelpers.Language.Spanish,
                    ConstantHelpers.Language.English
                };

                foreach (var userLanguage in userLanguages)
                {
                    if (supportedCultures.Any(y => userLanguage.Value.ToString().ToLower().Contains(y)))
                    {
                        var sopportedCulture = supportedCultures.Where(y => userLanguage.Value.ToString().ToLower().Contains(y)).FirstOrDefault();
                        language = sopportedCulture;
                        break;
                    }
                }

                return Task.FromResult(new ProviderCultureResult(language));
            }
            catch (Exception ex)
            {
                return Task.FromResult(new ProviderCultureResult(ConstantHelpers.Language.Spanish));
            }

        }
    }
}
