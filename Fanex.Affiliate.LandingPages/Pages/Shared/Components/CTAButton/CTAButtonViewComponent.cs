using Fanex.Affiliate.LandingPages.Core;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.CTAButton;

public class CTAButtonViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(LandingPageModel model)
    {
        var module = model?.LandingPage?.Modules?.FirstOrDefault(t => t.ModuleId == (int)UiModule.CTAButton);
        var mediaLink = module!.GetFullMediaLink(model!.GetMediaRootDomain());

        return View("CTAButton", mediaLink);
    }
}
