using System.Text;
using System.Text.Json;
using Fanex.Affiliate.LandingPages.Core.Constants;
using Fanex.Affiliate.LandingPages.Core.Services.Interfaces;

namespace Fanex.Affiliate.LandingPages.Core.Services;

public class HttpService(IHttpClientFactory httpClientFactory, ILogProvider logProvider) : IHttpService
{
    public async Task<(T?, HttpResponseMessage)> PostAsync<T>(string url, string content)
    {
        HttpResponseMessage? response = null;
        HttpRequestMessage? request = null;
        try
        {
            using var httpClient = httpClientFactory.CreateClient(Settings.UseProxyWithForwardHeaders);
            var requestContent = new StringContent(content, Encoding.UTF8, "application/json");
            request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = requestContent;
            response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<T>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            await WriteLogRequest(request, response);

            return (result, response);
        }
        catch
        {
            await WriteLogRequest(request!, response);
            throw;
        }
    }

    private async Task WriteLogRequest(HttpRequestMessage request, HttpResponseMessage? response)
    {
        var requestInfo = new StringBuilder();
        requestInfo.AppendLine($"[HTTP REQUEST] {request.Method} {request.RequestUri}");
        
        // Sanitize request headers
        foreach (var header in request.Headers)
        {
            var sanitizedValue = SensitiveDataSanitizer.SanitizeHeaderValue(header.Key, string.Join(", ", header.Value));
            requestInfo.AppendLine($"{header.Key}: {sanitizedValue}");
        }

        if (request.Content != null)
        {
            // Sanitize content headers
            foreach (var header in request.Content.Headers)
            {
                var sanitizedValue = SensitiveDataSanitizer.SanitizeHeaderValue(header.Key, string.Join(", ", header.Value));
                requestInfo.AppendLine($"{header.Key}: {sanitizedValue}");
            }

            var requestBody = await request.Content.ReadAsStringAsync();
            var sanitizedRequestBody = SensitiveDataSanitizer.SanitizeJsonContent(requestBody);
            requestInfo.AppendLine($"Body: {sanitizedRequestBody}");
        }
        
        if (response is not null)
        {
            requestInfo.AppendLine($"[HTTP RESPONSE] {(int)response.StatusCode} {response.ReasonPhrase}");

            // Sanitize response headers
            foreach (var header in response.Headers)
            {
                var sanitizedValue = SensitiveDataSanitizer.SanitizeHeaderValue(header.Key, string.Join(", ", header.Value));
                requestInfo.AppendLine($"{header.Key}: {sanitizedValue}");
            }

            if (response.Content != null)
            {
                // Sanitize content headers
                foreach (var header in response.Content.Headers)
                {
                    var sanitizedValue = SensitiveDataSanitizer.SanitizeHeaderValue(header.Key, string.Join(", ", header.Value));
                    requestInfo.AppendLine($"{header.Key}: {sanitizedValue}");
                }

                var responseBody = await response.Content.ReadAsStringAsync();
                var sanitizedResponseBody = SensitiveDataSanitizer.SanitizeJsonContent(responseBody);
                requestInfo.AppendLine($"Response Body: {sanitizedResponseBody}");
            }
        }

        logProvider.LogFileMessage(requestInfo.ToString());
    }
}
