using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeLogger.App.Web.Code.Common;

namespace TimeLogger.App.Web.Code.Report
{
    public class ReportCollectionResponse : ApiResponse
    {

        #region Properties

        [JsonProperty(PropertyName = "reportItems")]
        public ReportModel[] ReportItems { get; set; } = new ReportModel[] { };

        #endregion

    }
}