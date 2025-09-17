using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.LogoSponsorship;

public class LogoSponsorshipViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(LandingPageModel model)
    {
        var module = model.LandingPage!.CurrentModules?.FirstOrDefault();
        var mediaLink = module!.GetFullMediaLink(model!.GetMediaRootDomain());
        return View("LogoSponsorship", mediaLink);
    }
}
