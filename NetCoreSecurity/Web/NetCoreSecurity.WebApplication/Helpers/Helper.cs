using System.Net;
using System.Linq;
using System.Collections.Generic;

namespace NetCoreSecurity.WebApplication.Helpers
{
    public static class Helper
    {
        public static bool CheckWhiteListIP(string requestIp, List<string> validIPs)
        {
            return validIPs.Where(x => IPAddress.Parse(x).ToString().Equals(requestIp)).Any();
        }
    }
}