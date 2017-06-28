using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeLogger.App.Web.Code.Account
{
    public class UserModel : ICanValidate
    {

        #region Properties

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "plan")]
        public string Plan { get; set; }

        #endregion

        #region ICanValidate implementation

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Email)
                && !string.IsNullOrEmpty(Password)
                && !string.IsNullOrEmpty(Plan);
        }

        #endregion

    }
}