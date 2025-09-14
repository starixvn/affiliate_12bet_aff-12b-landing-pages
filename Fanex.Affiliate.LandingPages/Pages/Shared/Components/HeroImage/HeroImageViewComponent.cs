using Fanex.Affiliate.LandingPages.Core;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.HeroImage;

public class HeroImageViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(LandingPageModel model)
    {
        var module = model?.LandingPage?.Modules?.FirstOrDefault(t => t.ModuleId == (int)UiModule.HeroImage);
        var mediaLink = module!.GetFullMediaLink(model!.GetMediaRootDomain());
        return View("HeroImage", mediaLink);
    }
}
