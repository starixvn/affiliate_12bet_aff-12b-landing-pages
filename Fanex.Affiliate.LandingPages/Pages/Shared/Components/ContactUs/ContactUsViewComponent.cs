using Fanex.Affiliate.LandingPages.Core;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.ContactUs;

public class ContactUsViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(LandingPageModel model)
    {
        var module = model?.LandingPage?.Modules?.FirstOrDefault(t => t.ModuleId == (int)UiModule.ContactUs);
        var mediaLink = module!.GetFullMediaLink(model!.GetMediaRootDomain());
        var redirectLink = module?.ExtraInfo ?? string.Empty;

        return View("ContactUs", (redirectLink, mediaLink));
    }
}
