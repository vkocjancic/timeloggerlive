using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeLogger.App.Web.Code.Common;

namespace TimeLogger.App.Web.Code.Billing
{
    public class AccountListResponse : ApiResponse
    {

        #region Properties

        [JsonProperty(PropertyName = "account")]
        public AccountModel Account { get; set; }

        #endregion

    }
}