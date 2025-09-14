using Fanex.Affiliate.LandingPages.Core;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.ContactUs;

public class ContactUsViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(LandingPageModel model)
    {
        var contactUs = model?.LandingPage?.Modules?.FirstOrDefault(t => t.ModuleId == (int)UiModule.ContactUs);
        var redirectLink = contactUs?.ExtraInfo ?? string.Empty;
        var imageLink = contactUs!.GetFullMediaLink(model!.GetMediaRootDomain());
        return View("ContactUs", (redirectLink, imageLink));
    }
}
