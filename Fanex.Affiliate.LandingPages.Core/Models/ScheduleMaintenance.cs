namespace Fanex.Affiliate.LandingPages.Core.Models;

public class ScheduleMaintenance
{
    public bool Maintain { get; set; }
    public DateTimeOffset DateTimeStart { get; set; }
    public string UTCTimeStart { get; set; } = string.Empty;
    public DateTimeOffset DateTimeEnd { get; set; }
    public string UTCTimeEnd { get; set; } = string.Empty;
    public List<string>? ExcludeDomain { get; set; }
}
