using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeLogger.App.Web.Code.Account
{
    public class ResetPasswordModel : ICanValidate
    {
        #region Properties

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        #endregion

        #region ICanValidate implementation

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Id) 
                && !string.IsNullOrEmpty(Password);
        }

        #endregion

    }
}