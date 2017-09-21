using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using TimeLogger.App.Web.Code.Common;

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

        public Guid? UserId
        {
            get
            {
                return (User.Identity.IsAuthenticated) ? (Guid?)Membership.GetUser(User.Identity.Name).ProviderUserKey : null;
            }
        }

        #endregion

        #region Constructors

        public ControllerBase(Type classType)
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["mainDB"].ConnectionString;
            Log = LogManager.GetLogger(classType.FullName);
        }

        #endregion

        #region Methods

        protected void ExecuteCustomService(ApiResponse response, Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                response.ErrorDescription = ex.Message;
            }
        }

        #endregion

    }
}