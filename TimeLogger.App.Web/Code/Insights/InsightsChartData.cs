using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeLogger.App.Web.Code.Insights
{
    public class InsightsChartData
    {

        #region Properties

        public IEnumerable<InsightsChartDataSet> DataSets { get; set; }

        public IEnumerable<string> Labels { get; set; }

        #endregion

    }
}