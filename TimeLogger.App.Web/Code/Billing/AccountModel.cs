using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeLogger.App.Web.Code.Billing
{
    public class AccountModel
    {

        #region Properties

        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "created")]
        public DateTime Created { get; set; }

        [JsonProperty(PropertyName = "isactive")]
        public bool IsActive { get; set; }

        #endregion

        #region Extended properties

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "billingoption")]
        public string BillingOption { get; set; }

        [JsonProperty(PropertyName = "price")]
        public decimal Price { get; set; }

        #endregion

    }
}