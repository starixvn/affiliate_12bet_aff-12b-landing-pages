namespace Fanex.Affiliate.LandingPages.Pages;

using Fanex.Affiliate.LandingPages.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;

public class SignUpModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    [BindProperty]
    public SignupRequest Input { get; set; } = new SignupRequest();

    public string? ApiResponse { get; set; }

    public SignUpModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public void OnGet()
    {
        // load page lần đầu (chưa có dữ liệu)
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var client = _httpClientFactory.CreateClient();

        var response = await client.PostAsJsonAsync(
            "http://memberapi.uatasia.com/api/Site/SignupV2", Input);

        ApiResponse = await response.Content.ReadAsStringAsync();

        return Page();
    }
}
