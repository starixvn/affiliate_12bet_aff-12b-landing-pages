using Fanex.Data.Repository;

namespace Fanex.Affiliate.LandingPages.Core.Commands;

public abstract class BaseNonQueryCommand : NonQueryCommand
{
    public int ErrCode { get; set; }

    public string? ErrMsg { get; set; }
}