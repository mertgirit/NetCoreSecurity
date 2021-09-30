using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace NetCoreSecurity.WebApplication.Middleware
{
    using AppSettings;
    using NetCoreSecurity.WebApplication.Helpers;

    public class IPSecurityMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IPList _ipWhiteList;

        public IPSecurityMiddleware(RequestDelegate next, IOptions<IPList> ipWhiteList)
        {
            _next = next;
            _ipWhiteList = ipWhiteList.Value;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var requestIpAddress = httpContext.Connection.RemoteIpAddress;
            var isWhiteList = Helper.CheckWhiteListIP(requestIpAddress.ToString(), _ipWhiteList.WhiteList);
            if (!isWhiteList)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }

            await _next(httpContext);
        }
    }
}