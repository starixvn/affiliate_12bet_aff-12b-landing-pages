namespace Fanex.Affiliate.LandingPages.Core.Models;

public class LandingPageModule
{
    public int PageId { get; set; }

    public int ModuleId { get; set; }

    public int OrderId { get; set; }

    public int MediaId { get; set; }

    public string ExtraInfo { get; set; } = string.Empty;

    public string FileName { get; set; } = string.Empty;

    public string MediaName { get; set; } = string.Empty;

    public string Dimension { get; set; } = string.Empty;
}
