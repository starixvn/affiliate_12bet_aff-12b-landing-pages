using Fanex.Data;
using Fanex.Data.Repository;

namespace Fanex.Affiliate.LandingPages.Core.Criteria;

public class GetLandingPageCriteria : CriteriaBase
{
    public string PageCode { get; set; } = string.Empty;

    public bool IsPreview { get; set; }

    public override string GetSettingKey() => "Aff_LandingPage_Builder_Sel";

    public override bool IsValid() => !string.IsNullOrWhiteSpace(PageCode);
}