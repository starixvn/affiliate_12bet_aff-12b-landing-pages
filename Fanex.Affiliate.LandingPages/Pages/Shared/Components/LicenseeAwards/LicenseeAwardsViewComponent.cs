using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.LicenseeAwards;

public class LicenseeAwardsViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(LandingPageModel model)
    {
        var module = model.LandingPage!.CurrentModules?.FirstOrDefault();
        var mediaLink = module!.GetFullMediaLink(model.MediaRootDomain);
        return View("LicenseeAwards", mediaLink);
    }
}
