using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeLogger.App.Core.Insights;

namespace TimeLogger.App.Web.Code.Insights
{
    public class InsightsChartModel
    {

        #region Properties

        [JsonProperty(PropertyName = "startdate")]
        public string StartDate { get; set; }

        [JsonProperty(PropertyName = "enddate")]
        public string EndDate { get; set; }

        [JsonIgnore()]
        public Guid AccountId { get; set; }

        [JsonIgnore()]
        public DateTime Start {
            get
            {
                DateTime start;
                if ((!DateTime.TryParse(StartDate, out start))
                    || (DateTime.MinValue == start))
                {
                    start = DateTime.Today;
                }
                return start.Date;
            }
        }

        [JsonIgnore()]
        public DateTime End
        {
            get
            {
                DateTime end;
                if ((!DateTime.TryParse(EndDate, out end))
                    || (DateTime.MinValue == end))
                {
                    end = DateTime.Today;
                }
                return end.Date;
            }
        }

        [JsonIgnore()]
        public InsightsInterval Interval
        {
            get
            {
                var tsInterval = End - Start;
                if (6 == tsInterval.TotalDays)
                {
                    return InsightsInterval.Week;
                }
                if (31 >= tsInterval.TotalDays)
                {
                    return InsightsInterval.Month;
                }
                return InsightsInterval.Year;
            }
        }

        #endregion

    }
}