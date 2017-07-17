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

        #region Constructors

        public TimeLogController() : base(typeof(TimeLogController)) { }

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
                Log.Warn($"({User.Identity.Name}) Date parameter not set");
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var response = new TimeLogCollectionResponse() { Code = HttpStatusCode.InternalServerError, Success = false };
            try
            {
                Log.Debug($"({User.Identity.Name}) Obtaining user id");
                var userId = (Guid)Membership.GetUser(User.Identity.Name).ProviderUserKey;
                Log.Debug($"({User.Identity.Name}) Obtaining time logs for '{userId}'");
                response.TimeLogs = TimeLogService.GetAllFor(this.ConnectionString, DateTime.Parse(date), userId).ToArray();
                response.Code = HttpStatusCode.OK;
                response.Success = true;
                Log.Info($"({User.Identity.Name}) {response.TimeLogs.Length} time log(s) found for '{date}'");
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                response.ErrorDescription = ex.Message;
            }
            return Request.CreateResponse(response.Code, response);
        }

        // POST api/<controller>
        [Authorize]
        public HttpResponseMessage Post([FromBody]TimeLogModel model)
        {
            Log.Debug($"({User.Identity.Name}) Post method issued.");
            if (null == model)
            {
                Log.Warn($"({User.Identity.Name}) TimeLogModel not set");
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var response = new TimeLogResponse() { Code = HttpStatusCode.InternalServerError, Success = false };
            try
            {
                Log.Debug($"({User.Identity.Name}) Obtaining user id");
                model.AccountId = (Guid)Membership.GetUser(User.Identity.Name).ProviderUserKey;
                Log.Debug($"({User.Identity.Name}) Obtaining or creating assignment '{model.Description}' for account '{model.AccountId}");
                var assignment = AssignmentService.GetOrCreateAssignment(this.ConnectionString, model.Description, model.AccountId);
                Log.Debug($"({User.Identity.Name}) Assignment id '{assignment.Id}'");
                model.AssignmentId = assignment.Id;
                Log.Debug($"({User.Identity.Name}) Creating time log {model.From} - {model.To}");
                response = TimeLogService.CreateTimeLog(this.ConnectionString, model);
                Log.Info($"({User.Identity.Name}) TimeLog '{response.TimeLog.Id}' created");
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                response.ErrorDescription = ex.Message;
            }
            return Request.CreateResponse(response.Code, response);
        }

        // PUT api/<controller>
        [Authorize] 
        public HttpResponseMessage Put([FromBody]TimeLogModel model)
        {
            Log.Debug($"({User.Identity.Name}) Put method issued.");
            if (null == model)
            {
                Log.Warn($"({User.Identity.Name}) TimeLogModel not set");
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var response = new TimeLogResponse() { Code = HttpStatusCode.InternalServerError, Success = false };
            try
            {
                Log.Debug($"({User.Identity.Name}) Obtaining user id");
                model.AccountId = (Guid)Membership.GetUser(User.Identity.Name).ProviderUserKey;
                Log.Debug($"({User.Identity.Name}) Obtaining or creating assignment '{model.Description}' for account '{model.AccountId}");
                var assignment = AssignmentService.GetOrCreateAssignment(this.ConnectionString, model.Description, model.AccountId);
                Log.Debug($"({User.Identity.Name}) Assignment id '{assignment.Id}'");
                model.AssignmentId = assignment.Id;
                Log.Debug($"({User.Identity.Name}) Updateing time log '{model.Id}': {model.From} - {model.To}");
                response = TimeLogService.UpdateTimeLog(this.ConnectionString, model);
                Log.Info($"({User.Identity.Name}) TimeLog '{response.TimeLog.Id}' updated");
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                response.ErrorDescription = ex.Message;
            }
            return Request.CreateResponse(response.Code, response);
        }

        // DELETE api/<controller>/id
        [Authorize]
        public HttpResponseMessage Delete(Guid? id)
        {
            Log.Debug($"({User.Identity.Name}) Delete method issued.");
            if (null == id)
            {
                Log.Warn($"({User.Identity.Name}) TimeLog id not set");
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var response = new TimeLogResponse() { Code = HttpStatusCode.InternalServerError, Success = false };
            try
            {
                Log.Debug($"({User.Identity.Name}) Deleting timeLog '{id}'");
                response = TimeLogService.DeleteTimeLog(this.ConnectionString, id.Value);
                Log.Info($"({User.Identity.Name}) TimeLog '{response.TimeLog.Id}' deleted.");
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