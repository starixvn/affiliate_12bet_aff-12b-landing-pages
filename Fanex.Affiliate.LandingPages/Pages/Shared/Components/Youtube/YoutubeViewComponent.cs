using Fanex.Affiliate.LandingPages.Core;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.Youtube;

public class YoutubeViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(LandingPageModel model)
    {
        var youtubeLink = model?.LandingPage?.Modules?.FirstOrDefault(t => t.ModuleId == (int)UiModule.Youtube)?.ExtraInfo ?? string.Empty;
        youtubeLink = "https://www.youtube.com/watch?v=g6nCc-L0O7A&list=RDg6nCc-L0O7A&start_radio=1";
        return View("Youtube", youtubeLink);
    }
}
