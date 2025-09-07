namespace Fanex.Affiliate.LandingPages.Core.Services;

public class AffiliateCheckResponse
    {
        public string TraceId { get; set; }
        public string ErrorCodePrefix { get; set; }
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public string RawErrorCode { get; set; }
        public string RawMessage { get; set; }
        public int Data { get; set; }
    }
