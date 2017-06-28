using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using TimeLogger.App.Core.Notification;
using TimeLogger.App.Web.Code.Account;
using TimeLogger.App.Web.Code.Billing;

namespace TimeLogger.App.Web.Controllers
{
    public class RegisterController : ControllerBase
    {

        #region API commands

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody]UserModel model)
        {
            if ((null == model)
                || !model.IsValid())
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var response = new RegisterResponse() { Code = HttpStatusCode.InternalServerError, Success = false };
            try
            {
                response = RegisterService.CreateUser(model);
                if (response.Success)
                {
                    var domain = Domain;
                    var connectionString = ConnectionString;
                    Task.Factory.StartNew(() =>
                    {
                        var account = RegisterService.CreateAccount(connectionString, model);
                        BillingService.CreateBilling(connectionString, account, model.Plan);
                    });
                    Task.Factory.StartNew(() =>
                    {
                        var smtpSettings = new SmtpServerSettings(
                                ConfigurationManager.AppSettings["smtp.server"],
                                ConfigurationManager.AppSettings["smtp.port"],
                                ConfigurationManager.AppSettings["smtp.username"],
                                ConfigurationManager.AppSettings["smtp.password"],
                                domain
                            );
                        var notification = new RegistrationNotification(smtpSettings);
                        notification.Send(model.Email);
                    });
                }
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