using Fanex.Data.Repository;

namespace Fanex.Affiliate.LandingPages.Core.Criteria;

public class BaseCriteria(string spKey) : CriteriaBase
{
    public override string GetSettingKey() => spKey;

    public override bool IsValid() => !string.IsNullOrEmpty(spKey);
}