using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace TimeLogger.App.Web.Code.TimeLog
{
    public class TimeLogModel
    {

        #region Fields

        protected DateTime m_date = DateTime.MinValue;
        protected DateTime m_from = DateTime.MinValue;
        protected DateTime? m_to = null;
        protected Regex m_regexTimeParser = new Regex(@"(\d{1,2})[:.](\d{2})\s{0,1}(am|AM|pm|PM)?");

        #endregion

        #region Properties

        [JsonProperty(PropertyName = "id")]
        public Guid? Id { get; set; }

        [JsonProperty(PropertyName = "from")]
        public string From { get; set; }

        [JsonProperty(PropertyName = "to")]
        public string To { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "date")]
        public string Date { get; set; }

        [JsonIgnore]
        public Guid AccountId { get; set; }

        [JsonProperty(PropertyName = "duration")]
        public int Duration { get; set; }

        #endregion

        #region Public methods

        public DateTime GetDate()
        {
            if ((DateTime.MinValue == m_date)
                && (!DateTime.TryParse(Date, out m_date)))
            {
                m_date = DateTime.Today;
            }
            return m_date.Date;
        }

        public DateTime GetFrom()
        {
            if (DateTime.MinValue == m_from)
            {
                var tokens = ParseTimeFromString(From);
                if (null != tokens)
                { 
                    m_from = GetDate().AddHours(tokens.Item1 % 24).AddMinutes(tokens.Item2);
                }
            }
            return m_from;
        }

        public DateTime? GetTo()
        {
            if (!m_to.HasValue)
            {
                var tokens = ParseTimeFromString(To);
                if (null != tokens)
                {
                    m_to = GetDate().AddHours(tokens.Item1).AddMinutes(tokens.Item2);
                }
            }
            return m_to;
        }

        #endregion

        #region Protected methods

        protected Tuple<int, int> ParseTimeFromString(string time)
        {
            if (string.IsNullOrEmpty(time))
            {
                return null;
            }
            var match = m_regexTimeParser.Match(time);
            if (!match.Success)
            {
                return null;
            }
            var hours = int.Parse(match.Groups[1].Value);
            if ((4 == match.Groups.Count)
                && ("pm" == match.Groups[3].Value))
            {
                hours += 12;
            }
            return new Tuple<int, int>(hours, int.Parse(match.Groups[2].Value));
        }

        #endregion

    }
}