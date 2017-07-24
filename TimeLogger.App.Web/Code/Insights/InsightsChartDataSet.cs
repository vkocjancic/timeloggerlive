using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeLogger.App.Web.Code.Insights
{
    public class InsightsChartDataSet
    {

        #region Properties

        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }

        [JsonProperty(PropertyName = "data")]
        public IEnumerable<decimal> Data { get; set; }

        #endregion

    }
}