using Fanex.Affiliate.LandingPages.Core;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.LoginButton;

public class LoginButtonViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(LandingPageModel model)
    {
        var memberSiteLink = model.GetMemberSiteRedirectLink();
        var loginModule = model?.LandingPage?.Modules?.FirstOrDefault(t => t.ModuleId == (int)UiModule.LoginButton);
        var loginPath = loginModule?.ExtraInfo ?? "login";
        loginPath = loginPath.StartsWith('/') ? loginPath : $"/{loginPath}";
        var loginLink = $"{memberSiteLink}{loginPath}";
        var imageLink = loginModule?.FileName ?? string.Empty;
        return View("LoginButton", (loginLink, imageLink));
    }
}
