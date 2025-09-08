using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Affiliate.LandingPages.Core.Models;

public class BaseResponse
{
    public string TraceId { get; set; }
    public string ErrorCodePrefix { get; set; }
    public int ErrorCode { get; set; }
    public string Message { get; set; }
    public string RawErrorCode { get; set; }
    public string RawMessage { get; set; }
}
