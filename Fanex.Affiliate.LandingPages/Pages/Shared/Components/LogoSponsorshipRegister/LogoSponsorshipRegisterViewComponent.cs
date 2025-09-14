using Fanex.Affiliate.LandingPages.Core;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.LogoSponsorshipRegister;

public class LogoSponsorshipRegisterViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(LandingPageModel model)
    {
        var module = model?.LandingPage?.Modules?.FirstOrDefault(t => t.ModuleId == (int)UiModule.LogoSponsorshipRegister);
        var mediaLink = module!.GetFullMediaLink(model!.GetMediaRootDomain());
        return View("LogoSponsorshipRegister", mediaLink);
    }
}
