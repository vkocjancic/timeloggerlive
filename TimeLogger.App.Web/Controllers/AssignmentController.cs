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
    public class AssignmentController : ControllerBase
    {
        
        #region Constructors

        public AssignmentController() : base(typeof(AssignmentController))
        {
        }

        #endregion

        #region API methods

        // GET /App/api/assignment
        [Authorize]
        [HttpGet]
        public HttpResponseMessage Get()
        {
            Log.Debug($"({User.Identity.Name}) Get method issued.");
            var response = new AssingmentCollectionResponse() { Code = HttpStatusCode.InternalServerError, Success = false };
            try
            {
                Log.Debug($"({User.Identity.Name}) Obtaining user id");
                var userId = (Guid)Membership.GetUser(User.Identity.Name).ProviderUserKey;
                Log.Debug($"({User.Identity.Name}) Obtaining assignments for '{userId}'");
                response.Assignments = AssignmentService.GetAllFor(this.ConnectionString, userId).ToArray();
                response.Code = HttpStatusCode.OK;
                response.Success = true;
                Log.Info($"({User.Identity.Name}) {response.Assignments.Length} assignment(s) found");
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