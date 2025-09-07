namespace Fanex.Affiliate.LandingPages.Core
{
    public record CountryPhoneInfo(
        string Country,
        string Currency,
        string DialCode,
        int MinLength,
        int MaxLength
    );

    public static class CountryPhoneData
    {
        public static readonly List<CountryPhoneInfo> Countries = new()
        {
            new("Malaysia","MYR", "+60", 9, 10),
            new("Indonesia","IDR", "+62", 10, 13),
            new("China","CNY", "+86", 10, 11),
            new("India","INR", "+91", 10, 10),
            new("Thailand","THB", "+66", 9, 9),
            new("Bangladesh","BDT", "+880", 10, 10),
            new("Cambodia","KHR", "+855", 9, 9),
            new("Vietnam","VND", "+84", 9, 9),
            new("South Korea","KRW", "+82", 9, 10),
            new("Japan","JPY", "+81", 10, 10),
            new("Brazil","BRL", "+55", 10, 11),
            new("Philippines","PHP", "+63", 11, 11)
        };
    }
}
