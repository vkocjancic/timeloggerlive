using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using TimeLogger.App.Web.Code.Insights;

namespace TimeLogger.App.Web.Controllers
{

    public class InsightsChartController : ControllerBase
    {

        #region Constructors

        public InsightsChartController() : base(typeof(InsightsChartController)) { }

        #endregion

        #region API commands

        // POST api/<controller>
        [Authorize]
        public HttpResponseMessage Post([FromBody] InsightsChartModel model)
        {
            if (null == model)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var response = new InsightsChartResponse() { Code = HttpStatusCode.InternalServerError, Success = false };
            try
            {
                model.AccountId = (Guid)Membership.GetUser(User.Identity.Name).ProviderUserKey;
                var data = InsightsChartService.GetData(this.ConnectionString, model);
                response.DataSets = data.DataSets;
                response.Labels = data.Labels;
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