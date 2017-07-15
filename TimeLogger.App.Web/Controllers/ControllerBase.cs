using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace TimeLogger.App.Web.Controllers
{
    public class ControllerBase : ApiController
    {

        #region Properties

        protected Logger Log { get; set; }

        public string Domain
        {
            get
            {
                var host = HttpContext.Current.Request.Url.Host;
                if (host.StartsWith("www."))
                {
                    return host.Substring(4);
                }
                return host;
            }
        }

        public string ConnectionString { get; set; }

        #endregion

        #region Constructors

        public ControllerBase(Type classType)
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["mainDB"].ConnectionString;
            Log = LogManager.GetLogger(classType.FullName);
        }

        #endregion

    }
}