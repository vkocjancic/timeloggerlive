using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TimeLogger.Web.Core.Extensions
{
    public static class HttpRequestExtensions
    {

        public static bool IsAjaxRequest(this HttpRequestBase request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            return request.Path.Contains("/api/");
        }

    }
}
