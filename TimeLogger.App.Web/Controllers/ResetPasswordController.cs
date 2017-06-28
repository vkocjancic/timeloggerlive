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
        #region API commands

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody]ResetPasswordModel model)
        {
            if (null == model || !model.IsValid())
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var response = new ResetPasswordResponse() { Code = HttpStatusCode.InternalServerError, Success = false };
            try
            {
                response = ResetPasswordService.ResetPassword(ConnectionString, model);
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