using System.Text.Json;
using Fanex.Affiliate.LandingPages.Core.Models;
using Fanex.Affiliate.LandingPages.Core.Services.Interfaces;

namespace Fanex.Affiliate.LandingPages.Core.Services;

public class SignUpService(Logging.ILogger logger, 
    IMemberSiteConfigService memberSiteConfigService,
    IHttpClientFactory httpClientFactory) : ISignUpService
{
    public async Task SentryErrorAsync(string errMsg, Exception exception)
    {
        await logger.ErrorAsync(errMsg, exception);
    }
    
    public async Task<RecaptchaInfo> GetRecaptchaInfoAsync()
    {
        try
        {
            using var httpClient = httpClientFactory.CreateClient(Constants.UseProxyWithForwardHeaders);
            var request = new HttpRequestMessage(HttpMethod.Post, $"{memberSiteConfigService.GetAPIHost()}{ApiPath.GetRecaptchaInfo}");
            request.Headers.Add("accept", "text/plain");

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var jsonContent = await response.Content.ReadAsStringAsync();
            var recaptchaInfo = JsonSerializer.Deserialize<RecaptchaInfo>(jsonContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return recaptchaInfo ?? new RecaptchaInfo();
        }
        catch (Exception ex)
        {
            await logger.ErrorAsync("Error getting RecaptchaInfo", ex);
            return new RecaptchaInfo();
        }
    }

    public async Task<NumberCaptchaResponse> GetNumberCaptchaAsync()
    {
        try
        {
            using var httpClient = httpClientFactory.CreateClient(Constants.UseProxyWithForwardHeaders);
            var request = new HttpRequestMessage(HttpMethod.Post, $"{memberSiteConfigService.GetAPIHost()}{ApiPath.GetNumberCaptcha}")
            {
                Content = new StringContent("{\"action\":\"signup\"}", System.Text.Encoding.UTF8, "application/json")
            };

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<NumberCaptchaResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return result ?? new NumberCaptchaResponse
            {
                Data = new NumberCaptchaData()
            };
        }
        catch (Exception ex)
        {
            await logger.ErrorAsync("Error getting NumberCaptcha", ex);
            return new NumberCaptchaResponse
            {
                Data = new NumberCaptchaData()
            };
        }
    }

    public async Task<ExistsEmailResponse> CheckExistsEmailAsync(string email)
    {
        try
        {
            using var httpClient = httpClientFactory.CreateClient(Constants.UseProxyWithForwardHeaders);
            var apiHost = memberSiteConfigService.GetAPIHost();
            var requestUrl = $"{apiHost}{ApiPath.ExistsEmail}";

            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl)
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(new { email }),
                    System.Text.Encoding.UTF8,
                    "application/json")
            };

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ExistsEmailResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result ?? new ExistsEmailResponse();
        }
        catch (Exception ex)
        {
            await logger.ErrorAsync("Error checking ExistsEmail", ex);
            return new ExistsEmailResponse();
        }
    }

    public async Task<AffiliateCheckResponse> CheckAffiliateAsync(string affCode)
    {
        try
        {
            using var httpClient = httpClientFactory.CreateClient(Constants.UseProxyWithForwardHeaders);
            var apiHost = memberSiteConfigService.GetAPIHost();
            var requestUrl = $"{apiHost}{ApiPath.AffiliateCheck}";
            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl)
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(new { affCode }),
                    System.Text.Encoding.UTF8,
                    "application/json")
            };

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<AffiliateCheckResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result ?? new AffiliateCheckResponse();
        }
        catch (Exception ex)
        {
            await logger.ErrorAsync("Error checking Affiliate", ex);
            return new AffiliateCheckResponse();
        }
    }

    public async Task<ExistsUserNameResponse> CheckExistsUserNameAsync(string username, string lang)
    {
        try
        {
            using var httpClient = httpClientFactory.CreateClient(Constants.UseProxyWithForwardHeaders);
            var apiHost = memberSiteConfigService.GetAPIHost();
            var requestUrl = $"{apiHost}{ApiPath.ExistsUserName}/{lang}";

            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl)
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(new { username }),
                    System.Text.Encoding.UTF8,
                    "application/json")
            };

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ExistsUserNameResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result ?? new ExistsUserNameResponse();
        }
        catch (Exception ex)
        {
            await logger.ErrorAsync("Error checking ExistsUserName", ex);
            return new ExistsUserNameResponse();
        }
    }

    public async Task<BaseResponse> SignUpAsync(RegisterForm registerForm)
    {
        try
        {
            var signUpRequest = GetSignUpRequest(registerForm);
            using var httpClient = httpClientFactory.CreateClient(Constants.UseProxyWithForwardHeaders);
            var apiHost = memberSiteConfigService.GetAPIHost();
            var requestUrl = $"{apiHost}{ApiPath.SignupV2}/{signUpRequest.UserLang}";
            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl)
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(signUpRequest),
                    System.Text.Encoding.UTF8,
                    "application/json")
            };

            request.Headers.Add("User-Agent", registerForm.UserAgent);
            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<BaseResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            response.Headers.TryGetValues("Set-Cookie", out var cookies);
            if (result != null)
            {
                result.Cookies = cookies;
            }
            else
            {
                result = GetSignUpErrorResponse();
            }
            
            return result;
        }
        catch (Exception ex)
        {
            await logger.ErrorAsync("Error SignUpAsync", ex);
            return GetSignUpErrorResponse();
        }
    }

    private BaseResponse GetSignUpErrorResponse()
    {
        return new BaseResponse
        {
            ErrorCode = 9999,
            Message = "Something went wrong with your sign-up. Try again later or contact support."
        };
    }

    private SignUpRequest GetSignUpRequest(RegisterForm registerForm)
    {
        var signUp = new SignUpRequest
        {
            Username = registerForm.Username,
            UserPwd = registerForm.Password,
            Email = registerForm.Email,
            PhoneNo = registerForm.PhoneNo,
            CurrencyId = registerForm.CurrencyId,
            AreaCode = registerForm.AreaCode,
            AffCode = registerForm.AffCode,
            CaptchaType = registerForm.CaptchaType,
            ReCaptchaToken = registerForm.ReCaptchaToken,
            AffClickId = registerForm.AffClickId,
            ReferralUrl = registerForm.ReferralUrl,
            UserLang = registerForm.UserLang
        };

        return signUp;
    }
}
