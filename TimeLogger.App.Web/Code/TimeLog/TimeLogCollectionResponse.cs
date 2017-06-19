using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeLogger.App.Web.Code.Common;

namespace TimeLogger.App.Web.Code.TimeLog
{
    public class TimeLogCollectionResponse : ApiResponse
    {

        #region Properties

        [JsonProperty(PropertyName = "timelogs")]
        public TimeLogModel[] TimeLogs { get; set; } = new TimeLogModel[] { };

        #endregion

    }
}