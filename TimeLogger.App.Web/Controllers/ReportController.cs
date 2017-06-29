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

        #region API commands
        
        // GET api/<controller>/date
        [Authorize]
        [HttpGet]
        public HttpResponseMessage Get(string date)
        {
            if (null == date)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var response = new ReportCollectionResponse() { Code = HttpStatusCode.InternalServerError, Success = false };
            try
            {
                var userId = (Guid)Membership.GetUser(User.Identity.Name).ProviderUserKey;
                response.ReportItems = ReportService.GetAllFor(this.ConnectionString, DateTime.Parse(date), userId)
                    .OrderBy(r => r.Title)
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