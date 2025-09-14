using Fanex.Affiliate.LandingPages.Core;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.PromoList;

public class PromoListViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(LandingPageModel model)
    {
        var module = model?.LandingPage?.Modules?.FirstOrDefault(t => t.ModuleId == (int)UiModule.PromoList);
        var mediaLink = module!.GetFullMediaLink(model!.GetMediaRootDomain());
        return View("PromoList", mediaLink);
    }
}
