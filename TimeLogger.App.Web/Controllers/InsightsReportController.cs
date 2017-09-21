using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using TimeLogger.App.Web.Code.Insights;
using TimeLogger.App.Web.Code.Report;

namespace TimeLogger.App.Web.Controllers
{
    public class InsightsReportController : ControllerBase
    {
        
        #region Constructors

        public InsightsReportController() : base(typeof(InsightsReportController))
        {
        }

        #endregion

        #region API methods

        // POST api/<controller>
        [Authorize]
        public HttpResponseMessage Post([FromBody] InsightsReportModel model)
        {
            if (null == model)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var response = new ReportCollectionResponse() { Code = HttpStatusCode.InternalServerError, Success = false };
            try
            {
                model.AccountId = (Guid)Membership.GetUser(User.Identity.Name).ProviderUserKey;
                response.ReportItems = ReportService.GetAllFor(this.ConnectionString, model)
                    .OrderByDescending(r => r.Duration)
                    .ToArray();
                response.Code = HttpStatusCode.OK;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorDescription = ex.Message;
            }
            return Request.CreateResponse(response.Code, response);
        }

        #endregion

    }
}