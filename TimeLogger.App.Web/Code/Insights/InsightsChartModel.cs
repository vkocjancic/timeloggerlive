using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeLogger.App.Core.Insights;

namespace TimeLogger.App.Web.Code.Insights
{
    public class InsightsChartModel : InsightsModelBase
    {

        #region Properties       

        [JsonProperty(PropertyName = "interval")]
        public string Interval { get; set; }

        [JsonIgnore()]
        public InsightsInterval IntervalType
        {
            get
            {
                switch(Interval)
                {
                    case "M":
                        return InsightsInterval.Month;
                    case "Y":
                        return InsightsInterval.Year;
                    default:
                        return InsightsInterval.Week;
                }
            }
        }

        #endregion

    }
}