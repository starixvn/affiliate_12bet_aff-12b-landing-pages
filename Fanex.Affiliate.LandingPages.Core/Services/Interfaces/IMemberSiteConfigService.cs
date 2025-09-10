namespace Fanex.Affiliate.LandingPages.Core.Services.Interfaces;

public interface IMemberSiteConfigService
{
    string GetHost();

    string GetAPIHost();

    string GetFullLink(string path);

    string GetFullApiLink(string path);
}

