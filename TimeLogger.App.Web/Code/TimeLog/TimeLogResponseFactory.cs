using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeLogger.App.Web.Code.TimeLog
{
    public static class TimeLogResponseFactory
    {

        #region Public methods

        public static TimeLogResponse CreateFromModel(TimeLogModel model)
        {
            return new TimeLogResponse()
            {
                Code = System.Net.HttpStatusCode.Created,
                Success = true,
                TimeLog = model
            };
        }

        #endregion

    }
}