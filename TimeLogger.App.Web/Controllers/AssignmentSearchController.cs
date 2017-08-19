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
       
        #region Constructors

        public AssignmentSearchController() : base(typeof(AssignmentSearchController)) { }

        #endregion

        [Authorize]
        [HttpGet]
        public HttpResponseMessage Get(string query)
        {
            Log.Debug($"({User.Identity.Name}) Get method issued. query = '{query}'");
            var response = new AssignmentSearchCollectionResponse() { Code = HttpStatusCode.OK, Success = false, Assignments = new string[] { } };
            if (!string.IsNullOrEmpty(query)) {
                try
                {
                    Log.Debug($"({User.Identity.Name}) Obtaining user id");
                    var userId = (Guid)Membership.GetUser(User.Identity.Name).ProviderUserKey;
                    Log.Debug($"({User.Identity.Name}) Searching assignments for '{userId.ToString()}'");
                    response.Assignments = AssignmentService.SearchAllFor(this.ConnectionString, query, userId).Select(a => a.Description).ToArray();
                    response.Code = HttpStatusCode.OK;
                    response.Success = true;
                    Log.Info($"({User.Identity.Name}) {response.Assignments.Length} assignment(s) found for '{query}'");
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    response.ErrorDescription = ex.Message;
                }
            }
            return Request.CreateResponse(response.Code, response);
        }
    }

}