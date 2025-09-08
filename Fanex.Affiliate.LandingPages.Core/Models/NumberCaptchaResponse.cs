using Fanex.Affiliate.LandingPages.Core.Models;

namespace Fanex.Affiliate.LandingPages.Core.Services;

public class NumberCaptchaResponse : BaseResponse
{
    public NumberCaptchaData Data { get; set; }
}

public class NumberCaptchaData
{
    public int Seconds { get; set; }
    public string Picture { get; set; }
}