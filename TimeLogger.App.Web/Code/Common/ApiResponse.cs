using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace TimeLogger.App.Web.Code.Common
{

    public abstract class ApiResponse
    {

        #region Properties

        [JsonIgnore]
        public HttpStatusCode Code { get; set; }

        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }

        [JsonProperty(PropertyName = "errorDescription")]
        public string ErrorDescription { get; set; }

        #endregion

    }

}