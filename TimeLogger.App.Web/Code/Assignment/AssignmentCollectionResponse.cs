using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeLogger.App.Web.Code.Common;

namespace TimeLogger.App.Web.Code.Assignment
{
    public class AssingmentCollectionResponse : ApiResponse
    {

        #region Properties

        [JsonProperty(PropertyName = "assignments")]
        public string[] Assignments { get; set; }

        #endregion

    }
}