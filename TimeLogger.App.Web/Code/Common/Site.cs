using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace TimeLogger.App.Web.Code.Common
{
    public static class Site
    {

        #region Properties

        public static string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["mainDB"].ConnectionString; }
        }

        public static string Domain { get; private set; }

        #endregion

        #region Constructors

        static Site()
        {
            Domain = GetDomainNameFromHost(HttpContext.Current.Request.Url.Host);
        }

        private static string GetDomainNameFromHost(string host)
        {
            if (host.StartsWith("www."))
            {
                return host.Substring(4);
            }
            return host;
        }

        #endregion

    }
}