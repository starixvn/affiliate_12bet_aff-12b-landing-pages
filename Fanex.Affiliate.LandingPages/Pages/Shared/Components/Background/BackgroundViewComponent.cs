using Fanex.Affiliate.LandingPages.Core;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.Background;

public class BackgroundViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(LandingPageModel model)
    {
        var module = model?.LandingPage?.Modules?.FirstOrDefault(t => t.ModuleId == (int)UiModule.Background);
        var mediaLink = module!.GetFullMediaLink(model!.GetMediaRootDomain());

        return View("Background", mediaLink);
    }
}
