using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using TimeLogger.App.Web.Code.Assignment;

namespace TimeLogger.App.Web.Controllers
{

    public class AssignmentSearchController : ControllerBase
    {

        [Authorize]
        [HttpGet]
        public HttpResponseMessage Get(string query)
        {
            var response = new AssingmentCollectionResponse() { Code = HttpStatusCode.OK, Success = false, Assignments = new string[] { } };
            if (!string.IsNullOrEmpty(query)) {
                try
                {
                    var userId = (Guid)Membership.GetUser(User.Identity.Name).ProviderUserKey;
                    response.Assignments = AssignmentService.SearchAllFor(this.ConnectionString, query, userId).Select(a => a.Description).ToArray();
                    response.Code = HttpStatusCode.OK;
                    response.Success = true;
                }
                catch (Exception ex)
                {
                    response.ErrorDescription = ex.Message;
                }
            }
            return Request.CreateResponse(response.Code, response);
        }
    }

}