using System.Net;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace NetCoreSecurity.WebApplication.Middleware
{
    using AppSettings;

    public class IPSecurityMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IPList _ipWhiteList;

        public IPSecurityMiddleware(RequestDelegate next, IOptions<IPList> ipWhiteList)
        {
            this._next = next;
            this._ipWhiteList = ipWhiteList.Value;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var requestIpAddress = httpContext.Connection.RemoteIpAddress;
            var isWhiteList = _ipWhiteList.WhiteList.Where(x => IPAddress.Parse(x).Equals(requestIpAddress)).Any();
            if (!isWhiteList)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }

            await _next(httpContext);
        }
    }
}