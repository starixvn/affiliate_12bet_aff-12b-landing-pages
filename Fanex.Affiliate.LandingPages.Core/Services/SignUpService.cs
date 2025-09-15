using System.Diagnostics;
using System.Text.Json;
using Fanex.Affiliate.LandingPages.Core.Models;
using Fanex.Affiliate.LandingPages.Core.Resources;
using Fanex.Affiliate.LandingPages.Core.Services.Interfaces;

namespace Fanex.Affiliate.LandingPages.Core.Services;

public class SignUpService(Logging.ILogger logger, 
    IMemberSiteConfigService memberSiteConfigService,
    ITrackingSignUpService trackingSignUpService,
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

    public async Task<SignUpResponse> SignUpAsync(RegisterForm registerForm)
    {
        var apiHost = memberSiteConfigService.GetAPIHost();
        registerForm.ApiUrl = $"{apiHost}{ApiPath.SignupV2}/{registerForm.UserLang}";

        var stopwatch = Stopwatch.StartNew();
        var (signUpResponse, signUp) = await ProcessSignUpAPI(registerForm);
        stopwatch.Stop();
        
        var signUpRequest = new SignUpRequest
        {
            SignUpData = signUp,
            SignUpResponeData = signUpResponse,
            IpAddress = registerForm.IpAddress,
            UserAgent = registerForm.UserAgent,
            SignUpUrl = registerForm.SignUpUrl,
            ApiUrl = registerForm.ApiUrl,
            Duration = stopwatch.ElapsedMilliseconds,
        };

        _ = Task.Run(async () =>
        {
            await trackingSignUpService.WriteLogSignUp(signUpRequest);
        });
        
        return signUpResponse;
    }

    private async Task<(SignUpResponse, SignUp)> ProcessSignUpAPI(RegisterForm registerForm)
    {
        SignUpResponse result;
        var signUpRequest = GetSignUpRequest(registerForm);
        try
        {
            using var httpClient = httpClientFactory.CreateClient(Constants.UseProxyWithForwardHeaders);
            var request = new HttpRequestMessage(HttpMethod.Post, registerForm.ApiUrl)
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
            result = JsonSerializer.Deserialize<SignUpResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? GetSignUpErrorResponse();

            if (result.ErrorCode == 0)
            {
                response.Headers.TryGetValues("Set-Cookie", out var cookies);
                result.Cookies = cookies;
            }
        }
        catch (HttpRequestException ex)
        {
            await logger.ErrorAsync("Error SignUpAsync", ex);
            result = GetSignUpHttpRequestException(ex);
        }
        catch (Exception ex)
        {
            await logger.ErrorAsync("Error SignUpAsync", ex);
            result =  GetSignUpExceptionException(ex);
        }

        return (result, signUpRequest);
    }

    private SignUpResponse GetSignUpHttpRequestException(HttpRequestException ex)
    {
        return new SignUpResponse
        {
            ErrorCode = 9999,
            Message = UI.Validation_CommonError,
            RawErrorCode = ex.StatusCode?.ToString() ?? string.Empty,
            RawMessage = ex.Message
        };
    }

    private SignUpResponse GetSignUpExceptionException(Exception ex)
    {
        return new SignUpResponse
        {
            ErrorCode = 9999,
            Message = UI.Validation_CommonError,
            RawMessage = ex.Message
        };
    }

    private SignUpResponse GetSignUpErrorResponse()
    {
        return new SignUpResponse
        {
            ErrorCode = 9998,
            Message = UI.Validation_CommonError
        };
    }

    private SignUp GetSignUpRequest(RegisterForm registerForm)
    {
        var signUp = new SignUp
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
