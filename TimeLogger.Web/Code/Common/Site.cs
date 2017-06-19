using Resources;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;

namespace TimeLogger.Web.Code.Common
{
    public static class Site
    {

        #region Properties

        public static string Copyright { get; private set; }
        public static string Description { get; private set; }
        public static string Domain { get; private set; }
        public static string Email { get; private set; }
        public static string Image { get; private set; }
        public static string MetaDescription { get; private set; }
        public static string Title { get; private set; }
        public static string Theme { get; private set; }
        public static string UseGoogleAnalytics { get; private set; }
        public static string GoogleAnalyticsTrackerId { get; private set; }
        public static string Status { get; private set; }
        public static DateTime ReleaseDate { get; private set; }

        public static bool AreCookiesEnabled
        {
            get
            {
                var cookieBar = HttpContext.Current.Request.Cookies.Get("cookiebar");
                return ("true" == UseGoogleAnalytics) && (null != cookieBar) && ("CookieAllowed" == cookieBar.Value);
            }
        }

        public static string CurrentPage
        {
            get
            {
                var page = HttpUtility.HtmlEncode(HttpContext.Current.Request["page"]);
                if (string.IsNullOrEmpty(page))
                {
                    page = "Home";
                }
                return $"{page}.cshtml";
            }
        }

        public static string StatusSlug {
            get
            {
                return Status?.Replace(" ", "-").ToLower();
            }
        }

        #endregion

        #region Constructors

        static Site()
        {
            Copyright = ConfigurationManager.AppSettings.Get("site:copyright");
            Description = SiteResource.SiteDescription;
            Domain = GetDomainNameFromHost(HttpContext.Current.Request.Url.Host);
            Email = $"info@{Domain}";
            Image = ConfigurationManager.AppSettings.Get("site:image");
            MetaDescription = SiteResource.MetaSiteDescription;
            Theme = ConfigurationManager.AppSettings.Get("site:theme");
            Title = ConfigurationManager.AppSettings.Get("site:title");
            UseGoogleAnalytics = ConfigurationManager.AppSettings.Get("site:useGA");
            GoogleAnalyticsTrackerId = ConfigurationManager.AppSettings.Get("site:GATrackerId");
            Status = GetStatusString(ConfigurationManager.AppSettings.Get("app:status"));
            ReleaseDate = GetReleaseDate(ConfigurationManager.AppSettings.Get("app:releaseDate"));
        }

        private static string GetDomainNameFromHost(string host)
        {
            if (host.StartsWith("www."))
            {
                return host.Substring(4);
            }
            return host;
        }

        private static DateTime GetReleaseDate(string sReleaseDate)
        {
            DateTime dateRelease;
            if (!DateTime.TryParse(sReleaseDate, out dateRelease))
            {
                dateRelease = new DateTime(2018, 1, 1);
            }
            return dateRelease;
        }

        #endregion

        #region Private methods

        private static string GetStatusString(string status)
        {
            switch (status)
            {
                case "DEVELOPMENT":
                    return SiteResource.StatusInDevelopment;
                default:
                    return "";
            }
        }

        #endregion

    }
}