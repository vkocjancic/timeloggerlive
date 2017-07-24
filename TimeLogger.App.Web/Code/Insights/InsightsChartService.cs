using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeLogger.App.Core.Repository;

namespace TimeLogger.App.Web.Code.Insights
{
    public static class InsightsChartService
    {

        #region Public methods

        public static InsightsChartData GetData(string connectionString, InsightsChartModel model)
        {
            var repo = new InsightsChartRepository(connectionString);
            var data = repo.GetData(InsightsChartModelFactory.ToBusinessObject(model));
            return InsightsChartDataFactory.FromBusinessObject(data, model);
        }

        #endregion

    }
}