namespace Fanex.Affiliate.LandingPages.Core.Services;

public class NumberCaptchaResponse
{
    public string TraceId { get; set; }
    public string ErrorCodePrefix { get; set; }
    public int ErrorCode { get; set; }
    public string Message { get; set; }
    public string RawErrorCode { get; set; }
    public string RawMessage { get; set; }
    public NumberCaptchaData Data { get; set; }
}

public class NumberCaptchaData
{
    public int Seconds { get; set; }
    public string Picture { get; set; }
}