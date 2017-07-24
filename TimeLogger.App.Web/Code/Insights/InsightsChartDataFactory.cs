using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using TimeLogger.App.Core.Business;

namespace TimeLogger.App.Web.Code.Insights
{
    public static class InsightsChartDataFactory
    {

        #region Factory methods

        public static InsightsChartData FromBusinessObject(IEnumerable<Core.Business.InsightsChartData> data, InsightsChartModel model)
        {
            var dataHours = new List<decimal>();
            var dataTasks = new List<decimal>();
            var labels = new List<string>();
            if (Core.Insights.InsightsInterval.Year == model.Interval)
            {
                for (var dateTemp = model.Start; dateTemp <= model.End.AddDays(1); dateTemp = dateTemp.AddMonths(1))
                {
                    var entry = data.FirstOrDefault(e => e.Label.Date == new DateTime(dateTemp.Year, dateTemp.Month, 1));
                    if (null == entry)
                    {
                        dataHours.Add(0M);
                        dataTasks.Add(0M);
                    }
                    else
                    {
                        dataHours.Add(entry.Hours);
                        dataTasks.Add(entry.Tasks);
                    }
                    labels.Add(dateTemp.ToString("MMMM, yyyy", CultureInfo.CurrentUICulture));
                }
            }
            else
            {
                for (var dateTemp = model.Start; dateTemp <= model.End; dateTemp = dateTemp.AddDays(1))
                {
                    var entry = data.FirstOrDefault(e => e.Label.Date == dateTemp.Date);
                    if (null == entry)
                    {
                        dataHours.Add(0M);
                        dataTasks.Add(0M);
                    }
                    else
                    {
                        dataHours.Add(entry.Hours);
                        dataTasks.Add(entry.Tasks);
                    }
                    labels.Add(dateTemp.ToString("dd.MM.", CultureInfo.CurrentUICulture));
                }
            }
            return new InsightsChartData()
            {
                DataSets = new List<InsightsChartDataSet>() {
                    new InsightsChartDataSet() { Data = dataHours, Label = Resources.InsightsResources.Hours },
                    new InsightsChartDataSet() { Data = dataTasks, Label = Resources.InsightsResources.Tasks }
                },
                Labels = labels
            };
        }

        #endregion

    }
}