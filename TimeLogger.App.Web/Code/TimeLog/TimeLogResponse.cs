using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeLogger.App.Web.Code.Common;

namespace TimeLogger.App.Web.Code.TimeLog
{
    public class TimeLogResponse : ApiResponse
    {

        #region Properties

        [JsonProperty(PropertyName = "timelog")]
        public TimeLogModel TimeLog { get; set; }

        #endregion

    }
}