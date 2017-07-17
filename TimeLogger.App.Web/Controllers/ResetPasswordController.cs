using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;
using TimeLogger.App.Web.Code.Account;

namespace TimeLogger.App.Web.Controllers
{
    public class ResetPasswordController : ControllerBase
    {

        #region Constructors

        public ResetPasswordController() : base(typeof(ResetPasswordController)) { }

        #endregion

        #region API commands

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody]ResetPasswordModel model)
        {
            Log.Debug("Post method issued");
            if (!User.Identity.IsAuthenticated && (null == model || !model.IsValid()))
            {
                Log.Warn("Invalid reset password model");
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var response = new ResetPasswordResponse() { Code = HttpStatusCode.InternalServerError, Success = false };
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    Log.Debug($"({User.Identity.Name}) Obtaining user id");
                    var userId = (Guid)Membership.GetUser(User.Identity.Name).ProviderUserKey;
                    Log.Debug($"({User.Identity.Name}) Resetting password");
                    response = ResetPasswordService.ResetPasswordForAuthenticatedUser(ConnectionString, model, userId);
                    Log.Info($"({User.Identity.Name}) Password has been reset");
                }
                else
                {
                    Log.Debug($"({model.Id}) Resetting password");
                    response = ResetPasswordService.ResetPassword(ConnectionString, model);
                    Log.Info($"({model.Id}) Password has been reset");
                }
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