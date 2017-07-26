using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeLogger.App.Web.Code.Insights
{
    public abstract class InsightsModelBase
    {

        #region Properties

        [JsonProperty(PropertyName = "startdate")]
        public string StartDate { get; set; }

        [JsonProperty(PropertyName = "enddate")]
        public string EndDate { get; set; }

        [JsonIgnore()]
        public Guid AccountId { get; set; }

        [JsonIgnore()]
        public DateTime Start
        {
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

        #endregion

    }
}