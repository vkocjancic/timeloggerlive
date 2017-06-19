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

        #endregion

    }
}