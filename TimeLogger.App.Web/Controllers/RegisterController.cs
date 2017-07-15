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

        #region Constructors

        public RegisterController() : base(typeof(RegisterController)) { }

        #endregion

        #region API commands

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody]UserModel model)
        {
            Log.Debug("Post command issued");
            if ((null == model)
                || !model.IsValid())
            {
                Log.Warn("Invalid user model");
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var response = new RegisterResponse() { Code = HttpStatusCode.InternalServerError, Success = false };
            try
            {
                Log.Info($"({model.Email}) Creating user with plan '{model.Plan}'");
                response = RegisterService.CreateUser(model);
                if (response.Success)
                {
                    Log.Info($"({model.Email}) User created");
                    var domain = Domain;
                    var connectionString = ConnectionString;
                    Task.Factory.StartNew(() =>
                    {
                        Log.Debug($"({model.Email}) Creating account");
                        var account = RegisterService.CreateAccount(connectionString, model);
                        Log.Debug($"({model.Email}) Creating billing");
                        BillingService.CreateBilling(connectionString, account, model.Plan);
                        Log.Info($"({model.Email}) Account and billing created");
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
                        Log.Info($"({model.Email}) Registration notification sent");
                    });
                }
                else
                {
                    Log.Warn($"({model.Email}) Creating user failed. Reason: {response.ErrorDescription}");
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