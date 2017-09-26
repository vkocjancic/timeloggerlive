using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;
using TimeLogger.App.Web.Code.Assignment;
using TimeLogger.App.Web.Code.TimeLog;

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
        [Authorize(Roles = "User")]
        [HttpGet]
        public HttpResponseMessage Get()
        {
            Log.Debug($"({User.Identity.Name}) Get method issued.");
            var response = new AssignmentCollectionResponse() { Code = HttpStatusCode.InternalServerError, Success = false };
            try
            {
                Log.Debug($"({User.Identity.Name}) Obtaining assignments for '{this.UserId}'");
                response.Assignments = AssignmentService.GetAllFor(this.ConnectionString, this.UserId.Value).OrderBy(a => a.Description).ToArray();
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

        [Authorize(Roles = "User")]
        [HttpGet]
        public HttpResponseMessage Get(Guid id)
        {
            Log.Debug($"({User.Identity.Name}) Get method issued for taks '{id}'.");
            var response = new AssignmentResponse() { Code = HttpStatusCode.InternalServerError, Success = false };
            try
            {
                Log.Debug($"({User.Identity.Name}) Obtaining time logs for '{this.UserId}'");
                response.TimeLogs = TimeLogService.GetAllForTask(ConnectionString, id, this.UserId.Value)
                    .OrderBy(t => t.Description)
                    .ToArray();
                response.Code = HttpStatusCode.OK;
                response.Success = true;
                Log.Info($"({User.Identity.Name}) {response.TimeLogs.Length} time log(s) found");
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                response.ErrorDescription = ex.Message;
            }
            return Request.CreateResponse(response.Code, response);
        }

        [Authorize(Roles = "User")]
        public HttpResponseMessage Put([FromBody] AssignmentModel model)
        {
            Log.Debug($"({User.Identity.Name}) Put method issued.");
            var response = new AssignmentResponse() { Code = HttpStatusCode.InternalServerError, Success = false };
            ExecuteCustomService(response, () =>
            {
                model.UserId = this.UserId;               
                var assignmentLinked = AssignmentService.GetByDescription(this.ConnectionString, model.Description, model.UserId.Value);
                if (null == assignmentLinked)
                {
                    Log.Debug($"({User.Identity.Name}) Updating assignment '{model.Id}' for '{model.UserId}'");
                    AssignmentService.UpdateAssignment(this.ConnectionString, model);
                }
                else
                {
                    model.WasMerged = true;
                    Log.Debug($"({User.Identity.Name}) Linking to assignment '{assignmentLinked.Id}' for '{model.UserId}'");
                }
                if (!string.IsNullOrEmpty(model.DescriptionOld) && (model.Description != model.DescriptionOld))
                {
                    TimeLogService.UpdateAllForTask(this.ConnectionString, model.Id, model.UserId.Value, (timelog) =>
                    {
                        timelog.Description = model.Description;
                        if (model.WasMerged)
                        {
                            timelog.AssignmentId = assignmentLinked.Id;
                        }
                    });
                    if (model.WasMerged)
                    {
                        Log.Debug($"({User.Identity.Name}) Deleting assignment '{model.Id}' for '{model.UserId}'");
                        AssignmentService.Delete(this.ConnectionString, model.Id, model.UserId.Value);
                    }
                }
                response.Code = HttpStatusCode.OK;
                response.Success = true;
                response.WasMerged = model.WasMerged;
                Log.Info($"({User.Identity.Name}) Assignment '{model.Id}' updated");
            });
            return Request.CreateResponse(response.Code, response);
        }

        #endregion

    }
}