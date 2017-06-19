using System;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;

namespace TimeLogger.Web.Core
{
    public static class Site
    {

        #region Public methods

        public static string FingerPrint(string rootRelativePath)
        {
            if (null == HttpRuntime.Cache[rootRelativePath])
            {
                var relative = VirtualPathUtility.ToAbsolute("~" + rootRelativePath);
                var absolute = HostingEnvironment.MapPath(relative);
                if (!File.Exists(absolute))
                    throw new FileNotFoundException("File not found", absolute);
                var date = File.GetLastWriteTime(absolute);
                var index = relative.LastIndexOf('.');
                var result = relative.Insert(index, "_" + date.Ticks);
                HttpRuntime.Cache.Insert(rootRelativePath, result, new CacheDependency(absolute));
            }
            return HttpRuntime.Cache[rootRelativePath] as string;
        }

        public static void SetConditionalGetHeaders(DateTime dateLastModified, HttpContextBase context)
        {
            var response = context.Response;
            var request = context.Request;
            dateLastModified = new DateTime(dateLastModified.Year, dateLastModified.Month, dateLastModified.Day, dateLastModified.Hour, dateLastModified.Minute, dateLastModified.Second);
            var incomingDate = request.Headers["If-Modified-Since"];
            response.Cache.SetLastModified(dateLastModified);
            var dateToTest = DateTime.MinValue;
            if (DateTime.TryParse(incomingDate, out dateToTest) && dateToTest == dateLastModified)
            {
                response.ClearContent();
                response.StatusCode = (int)System.Net.HttpStatusCode.NotModified;
                response.SuppressContent = true;
            }
        }

        #endregion

    }
}
