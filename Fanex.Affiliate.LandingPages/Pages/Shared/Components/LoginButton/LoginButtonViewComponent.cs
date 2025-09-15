using Fanex.Affiliate.LandingPages.Core;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.LoginButton;

public class LoginButtonViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(LandingPageModel model)
    {
        var memberSiteLink = model.GetMemberSiteRedirectLink();
        var module = model?.LandingPage?.Modules?.FirstOrDefault(t => t.ModuleId == (int)UiModule.LoginButton);
        var loginPath = module?.ExtraInfo ?? "login";
        loginPath = loginPath.StartsWith('/') ? loginPath : $"/{loginPath}";
        var loginLink = $"{memberSiteLink}{loginPath}";
        var mediaLink = module!.GetFullMediaLink(model!.GetMediaRootDomain());

        return View("LoginButton", (loginLink, mediaLink));
    }
}
