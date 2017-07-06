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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        // GET api/<controller>/accounttype
        public HttpResponseMessage Get(string accountType)
        {
            if (null == accountType)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var response = new AccountListCollectionResponse() { Code = HttpStatusCode.InternalServerError, Success = false };
            try
            {
                response.Accounts = AccountService.GetAllFor(this.ConnectionString, accountType).ToArray();
                response.Code = HttpStatusCode.OK;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorDescription = ex.Message;
            }
            return Request.CreateResponse(response.Code, response);
        }

        // PUT api/<controller>
        [Authorize(Roles = "Admin")]
        public HttpResponseMessage Put([FromBody]AccountModel model)
        {
            if (null == model)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var response = new AccountListResponse() { Code = HttpStatusCode.InternalServerError, Success = false };
            try
            {
                response = AccountService.ActivateAccount(this.ConnectionString, model);
            }
            catch (Exception ex)
            {
                response.ErrorDescription = ex.Message;
            }
            return Request.CreateResponse(response.Code, response);
        }

    }
}