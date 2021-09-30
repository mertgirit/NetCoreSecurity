using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NetCoreSecurity.WebApplication.Filters
{
    using AppSettings;
    using NetCoreSecurity.WebApplication.Helpers;

    public class CheckWhiteList : ActionFilterAttribute
    {
        private readonly IPList _ipList;

        public CheckWhiteList(IOptions<IPList> ipList)
        {
            _ipList = ipList.Value;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (Helper.CheckWhiteListIP(context.HttpContext.Connection.RemoteIpAddress.ToString(), _ipList.WhiteList))
            {
                context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}