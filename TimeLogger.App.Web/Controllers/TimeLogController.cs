using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using TimeLogger.App.Web.Code.Assignment;
using TimeLogger.App.Web.Code.TimeLog;

namespace TimeLogger.App.Web.Controllers
{
    public class TimeLogController : ControllerBase
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
            var response = new TimeLogCollectionResponse() { Code = HttpStatusCode.InternalServerError, Success = false };
            try
            {
                var userId = (Guid)Membership.GetUser(User.Identity.Name).ProviderUserKey;
                response.TimeLogs = TimeLogService.GetAllFor(this.ConnectionString, DateTime.Parse(date), userId).ToArray();
                response.Code = HttpStatusCode.OK;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorDescription = ex.Message;
            }
            return Request.CreateResponse(response.Code, response);
        }

        // POST api/<controller>
        [Authorize]
        public HttpResponseMessage Post([FromBody]TimeLogModel model)
        {
            if (null == model)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var response = new TimeLogResponse() { Code = HttpStatusCode.InternalServerError, Success = false };
            try
            {
                model.AccountId = (Guid)Membership.GetUser(User.Identity.Name).ProviderUserKey;
                var assignment = AssignmentService.GetOrCreateAssignment(this.ConnectionString, model.Description, model.AccountId);
                model.AssignmentId = assignment.Id;
                response = TimeLogService.CreateTimeLog(this.ConnectionString, model);
            }
            catch (Exception ex)
            {
                response.ErrorDescription = ex.Message;
            }
            return Request.CreateResponse(response.Code, response);
        }

        // PUT api/<controller>
        [Authorize] 
        public HttpResponseMessage Put([FromBody]TimeLogModel model)
        {
            if (null == model)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var response = new TimeLogResponse() { Code = HttpStatusCode.InternalServerError, Success = false };
            try
            {
                model.AccountId = (Guid)Membership.GetUser(User.Identity.Name).ProviderUserKey;
                var assignment = AssignmentService.GetOrCreateAssignment(this.ConnectionString, model.Description, model.AccountId);
                model.AssignmentId = assignment.Id;
                response = TimeLogService.UpdateTimeLog(this.ConnectionString, model);
            }
            catch (Exception ex)
            {
                response.ErrorDescription = ex.Message;
            }
            return Request.CreateResponse(response.Code, response);
        }

        // DELETE api/<controller>/id
        [Authorize]
        public HttpResponseMessage Delete(Guid? id)
        {
            if (null == id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var response = new TimeLogResponse() { Code = HttpStatusCode.InternalServerError, Success = false };
            try
            {
                response = TimeLogService.DeleteTimeLog(this.ConnectionString, id.Value);
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