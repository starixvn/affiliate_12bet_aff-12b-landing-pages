namespace Fanex.Affiliate.LandingPages.Core.Services.Interfaces;

public interface ICacheKeyService
{
    string GetLandingPageKey(string pageCode);
    string GetLandingPagePreviewKey(string pageCode, int pageId);
    string GetAllLandingPageVersionKey();
    string GetSignUpKey();
    string GetRecaptchaInfoKey(string langCode);

    string GetUnderMaintainInfoKey();
}

