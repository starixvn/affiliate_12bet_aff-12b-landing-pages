using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.CTAButton;

public class CTAButtonViewComponent: ViewComponent
{
    public IViewComponentResult Invoke(LandingPageModel model)
    {
        var module = model.LandingPage!.CurrentModules?.FirstOrDefault();
        var mediaLink = module!.GetFullMediaLink(model.MediaRootDomain);

        return View("CTAButton", mediaLink);
    }
}
