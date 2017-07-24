using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeLogger.App.Web.Code.Insights
{
    public static class InsightsChartModelFactory
    {

        #region Factory methods

        public static Core.Business.InsightsChart ToBusinessObject(InsightsChartModel model)
        {
            return new Core.Business.InsightsChart()
            {
                UserId = model.AccountId,
                StartDate = model.Start,
                EndDate = model.End,
                Interval = model.Interval
            };
        }

        #endregion

    }
}