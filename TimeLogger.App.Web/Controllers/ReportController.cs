using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using TimeLogger.App.Web.Code.Report;

namespace TimeLogger.App.Web.Controllers
{
    public class ReportController : ControllerBase
    {

        #region Constructors

        public ReportController() : base(typeof(ReportController)) { }

        #endregion

        #region API commands

        // GET api/<controller>/date
        [Authorize]
        [HttpGet]
        public HttpResponseMessage Get(string date)
        {
            Log.Debug($"({User.Identity.Name}) Get method issued. date = '{date}'");
            if (null == date)
            {
                Log.Warn($"({User.Identity.Name}) Date was not set");
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var response = new ReportCollectionResponse() { Code = HttpStatusCode.InternalServerError, Success = false };
            try
            {
                Log.Debug($"({User.Identity.Name}) Obtaining user id");
                var userId = (Guid)Membership.GetUser(User.Identity.Name).ProviderUserKey;
                Log.Debug($"({User.Identity.Name}) Obtaining report for '{userId.ToString()}'");
                response.ReportItems = ReportService.GetAllFor(this.ConnectionString, DateTime.Parse(date), userId)
                    .OrderBy(r => r.Title)
                    .ToArray();
                response.Code = HttpStatusCode.OK;
                response.Success = true;
                Log.Info($"({User.Identity.Name}) {response.ReportItems.Length} report items found for '{date}'");
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                response.ErrorDescription = ex.Message;
            }
            return Request.CreateResponse(response.Code, response);
        } 

        #endregion

    }
}