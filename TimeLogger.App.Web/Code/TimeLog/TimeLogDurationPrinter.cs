using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeLogger.App.Web.Code.TimeLog
{

    public static class TimeLogDurationPrinter
    {

        #region Public methods

        public static string Display(int duration)
        {
            var tsDuration = new TimeSpan(0, duration, 0);
            return $"{tsDuration.Hours}h {tsDuration.Minutes}m";
        }

        #endregion

    }

}