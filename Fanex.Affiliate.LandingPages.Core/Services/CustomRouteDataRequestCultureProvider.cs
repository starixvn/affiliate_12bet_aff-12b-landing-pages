using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using Fanex.Affiliate.LandingPages.Core.LandingPageConstants;

namespace Fanex.Affiliate.LandingPages.Core.Services;

/// <summary>
/// Custom culture provider that maps language codes from route to culture codes
/// </summary>
public class CustomRouteDataRequestCultureProvider : RequestCultureProvider
{
    public override Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext httpContext)
    {
        if (httpContext == null)
        {
            return Task.FromResult<ProviderCultureResult?>(null);
        }

        var routeValues = httpContext.Request.RouteValues;
        
        if (routeValues.TryGetValue("languageCode", out var languageCodeObj))
        {
            var languageCode = languageCodeObj?.ToString();
            
            if (!string.IsNullOrEmpty(languageCode))
            {
                // Map language code to culture code
                var cultureName = CultureMappingConstants.GetCultureName(languageCode);
                return Task.FromResult<ProviderCultureResult?>(new ProviderCultureResult(cultureName, cultureName));
            }
        }

        return Task.FromResult<ProviderCultureResult?>(null);
    }
}
