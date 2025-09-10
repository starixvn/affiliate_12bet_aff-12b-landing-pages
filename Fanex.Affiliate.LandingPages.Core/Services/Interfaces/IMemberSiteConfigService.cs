namespace Fanex.Affiliate.LandingPages.Core.Services.Interfaces;

public interface IMemberSiteConfigService
{
    string GetHost();

    string GetAPIHost();

    string GetFullUrl(string path);

    string GetFullApiUrl(string path);
}

