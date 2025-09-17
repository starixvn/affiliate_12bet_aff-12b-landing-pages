using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.Background;

public class BackgroundViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(LandingPageModel model)
    {
        var module = model.LandingPage!.CurrentModules?.FirstOrDefault();
        var mediaLink = module!.GetFullMediaLink(model!.GetMediaRootDomain());

        return View("Background", mediaLink);
    }
}
