using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
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
            if (null == model || !model.IsValid())
            {
                Log.Warn("Invalid reset password model");
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var response = new ResetPasswordResponse() { Code = HttpStatusCode.InternalServerError, Success = false };
            try
            {
                Log.Debug($"({model.Id}) Resetting password");
                response = ResetPasswordService.ResetPassword(ConnectionString, model);
                Log.Info($"({model.Id}) Password has been reset");
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