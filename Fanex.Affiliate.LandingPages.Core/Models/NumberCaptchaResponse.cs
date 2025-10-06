using Fanex.Affiliate.LandingPages.Core.Models;

namespace Fanex.Affiliate.LandingPages.Core.Services;

public class NumberCaptchaResponse : BaseResponse
{
    public NumberCaptchaData? Data { get; set; }
}

public class NumberCaptchaData
{
    public int Seconds { get; set; }
    public string Picture { get; set; } = string.Empty;
}

public class NumberCaptchaExpire
{
    public NumberCaptchaExpire(NumberCaptchaData numberCaptchaData)
    {
        NumberCaptchaData = numberCaptchaData;
        ExpireTime = DateTime.Now.AddSeconds(numberCaptchaData.Seconds);
    }

    public NumberCaptchaData NumberCaptchaData { get; set; }

    public DateTime ExpireTime { get; set; }
}