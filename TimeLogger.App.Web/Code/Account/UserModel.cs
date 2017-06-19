using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeLogger.App.Web.Code.Account
{
    public class UserModel
    {

        #region Properties

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "plan")]
        public string Plan { get; set; }

        #endregion

    }
}