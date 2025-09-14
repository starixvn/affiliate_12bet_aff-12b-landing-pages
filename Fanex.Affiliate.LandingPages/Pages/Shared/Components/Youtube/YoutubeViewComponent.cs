using Fanex.Affiliate.LandingPages.Core;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.Youtube;

public class YoutubeViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(LandingPageModel model)
    {
        var youtubeLink = model?.LandingPage?.Modules?.FirstOrDefault(t => t.ModuleId == (int)UiModule.Youtube)?.ExtraInfo ?? string.Empty;
        return View("Youtube", youtubeLink);
    }
}
