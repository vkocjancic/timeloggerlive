using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TimeLogger.App.Web.Code.Billing;

namespace TimeLogger.App.Web.Controllers
{
    public class AccountListController : ControllerBase
    {

        #region Constructors

        public AccountListController() : base(typeof(AccountListController)) { }

        #endregion

        [Authorize(Roles = "Admin")]
        [HttpGet]
        // GET api/<controller>/accounttype
        public HttpResponseMessage Get(string accountType)
        {
            Log.Debug($"({User.Identity.Name}) Get method issued. accountType = '{accountType}'");
            if (null == accountType)
            {
                Log.Warn($"({User.Identity.Name}) Account type parameter not set");
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var response = new AccountListCollectionResponse() { Code = HttpStatusCode.InternalServerError, Success = false };
            try
            {
                Log.Debug($"({User.Identity.Name}) Obtaining accounts");
                response.Accounts = AccountService.GetAllFor(this.ConnectionString, accountType).ToArray();
                response.Code = HttpStatusCode.OK;
                response.Success = true;
                Log.Info($"({User.Identity.Name}) {response.Accounts.Length} account(s) found for '{accountType}'");
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                response.ErrorDescription = ex.Message;
            }
            return Request.CreateResponse(response.Code, response);
        }

        // PUT api/<controller>
        [Authorize(Roles = "Admin")]
        public HttpResponseMessage Put([FromBody]AccountModel model)
        {
            Log.Debug($"({User.Identity.Name}) Put method issued.");
            if (null == model)
            {
                Log.Warn($"({User.Identity.Name}) AccountModel not set");
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var response = new AccountListResponse() { Code = HttpStatusCode.InternalServerError, Success = false };
            try
            {
                Log.Debug($"({User.Identity.Name}) Activating account for '{model.Id}'");
                response = AccountService.ActivateAccount(this.ConnectionString, model);
                Log.Info($"({User.Identity.Name}) Account '{response.Account.Id}' updated");
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                response.ErrorDescription = ex.Message;
            }
            return Request.CreateResponse(response.Code, response);
        }

    }
}