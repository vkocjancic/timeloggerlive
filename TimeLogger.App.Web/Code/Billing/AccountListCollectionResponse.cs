using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeLogger.App.Web.Code.Common;

namespace TimeLogger.App.Web.Code.Billing
{
    public class AccountListCollectionResponse : ApiResponse
    {

        #region Properties

        [JsonProperty(PropertyName = "accounts")]
        public AccountModel[] Accounts { get; set; } = new AccountModel[] { };

        #endregion

    }
}