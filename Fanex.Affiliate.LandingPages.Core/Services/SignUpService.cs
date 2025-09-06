using Fanex.Affiliate.LandingPages.Core.Services.Interfaces;

namespace Fanex.Affiliate.LandingPages.Core.Services;

public class SignUpService(Logging.ILogger logger, IMemberSiteConfigService memberSiteConfig) : ISignUpService
{
    public async Task SentryErrorAsync(string errMsg, Exception exception)
    {
        await logger.ErrorAsync(errMsg, exception);
    }

    /// <summary>
    /// Example method showing how to use MemberSite configuration
    /// </summary>
    public string GetMemberSiteLoginUrl()
    {
        // Example: Get the login URL by combining host with login path
        return memberSiteConfig.GetFullUrl("login");
    }

    /// <summary>
    /// Example method showing how to use MemberSite API configuration
    /// </summary>
    public string GetMemberSiteApiEndpoint(string endpoint)
    {
        // Example: Get API endpoint URL
        return memberSiteConfig.GetFullApiUrl($"api/{endpoint}");
    }

    /// <summary>
    /// Example method showing direct access to configuration values
    /// </summary>
    public (string Host, string ApiHost) GetMemberSiteInfo()
    {
        return (memberSiteConfig.GetHost(), memberSiteConfig.GetAPIHost());
    }
}
