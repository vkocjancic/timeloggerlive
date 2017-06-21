using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeLogger.App.Web.Code.Report
{
    public class ReportModel
    {

        #region Properties

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "duration")]
        public int Duration { get; set; } 

        [JsonProperty(PropertyName = "durationtext")]
        public string DurationText
        {
            get
            {
                var ts = new TimeSpan(0, Duration, 0);
                return $"{ts.Hours}h {ts.Minutes}m";
            }
        }

        #endregion

    }
}