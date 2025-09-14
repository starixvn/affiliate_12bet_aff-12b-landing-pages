using Fanex.Affiliate.LandingPages.Core;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.PaymentMethod;

public class PaymentMethodViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(LandingPageModel model)
    {
        var module = model?.LandingPage?.Modules?.FirstOrDefault(t => t.ModuleId == (int)UiModule.PaymentMethod);
        var mediaLink = module!.GetFullMediaLink(model!.GetMediaRootDomain());
        return View("PaymentMethod", mediaLink);
    }
}
