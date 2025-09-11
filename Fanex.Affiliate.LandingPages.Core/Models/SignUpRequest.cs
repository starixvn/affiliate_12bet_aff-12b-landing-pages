namespace Fanex.Affiliate.LandingPages.Core.Models;

public class SignUpRequest
{
    public SignUp? SignUpData { get; set; }

    public BaseResponse? SignUpResponeData { get; set; }

    public string IpAddress { get; set; } = string.Empty;

    public long Duration { get; set; }

    public string UserAgent { get; set; } = string.Empty;

    public string ApiUrl { get; set; } = string.Empty;

    public string SignUpUrl { get; set; } = string.Empty;

}
