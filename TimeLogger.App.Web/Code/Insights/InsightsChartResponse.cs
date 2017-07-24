using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeLogger.App.Web.Code.Common;

namespace TimeLogger.App.Web.Code.Insights
{
    public class InsightsChartResponse : ApiResponse
    {

        [JsonProperty(PropertyName = "datasets")]
        public IEnumerable<InsightsChartDataSet> DataSets { get; set; }

        [JsonProperty(PropertyName = "labels")]
        public IEnumerable<string> Labels { get; set; }

    }
}