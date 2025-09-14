using Fanex.Affiliate.LandingPages.Core;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.LicenseeAwards;

public class LicenseeAwardsViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(LandingPageModel model)
    {
        var module = model?.LandingPage?.Modules?.FirstOrDefault(t => t.ModuleId == (int)UiModule.LicenseeAwards);
        var mediaLink = module!.GetFullMediaLink(model!.GetMediaRootDomain());
        return View("LicenseeAwards", mediaLink);
    }
}
