using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeLogger.App.Core.Repository;
using TimeLogger.App.Web.Code.Adapter;

namespace TimeLogger.App.Web.Code.TimeLog
{
    public static class TimeLogService
    {

        #region Fields

        private static string m__rosTimeFormat = "H:mm";

        #endregion

        #region Public methods

        public static TimeLogResponse CreateTimeLog(string connectionString, TimeLogModel timeLog)
        {
            var repo = new TimeLogRepository(connectionString);
            if (null == timeLog.Id)
            {
                timeLog.Id = Guid.NewGuid();
            }
            var adapter = new TimeLogAdapter();
            var timeLogData = repo.CreateTimeLog(adapter.ToBusinessObject(timeLog));
            timeLog.From = timeLogData.From.ToString(m__rosTimeFormat);
            timeLog.To = timeLogData.To?.ToString(m__rosTimeFormat);
            timeLog.Duration = (null == timeLogData.To) ? 0 : (int)(timeLogData.To.Value - timeLogData.From).TotalMinutes;
            return TimeLogResponseFactory.CreateFromModel(timeLog);
        }

        public static TimeLogResponse DeleteTimeLog(string connectionString, Guid id)
        {
            var repo = new TimeLogRepository(connectionString);
            repo.DeleteTimeLog(id);
            return new TimeLogResponse()
            {
                Code = System.Net.HttpStatusCode.OK,
                Success = true
            };
        }

        public static IEnumerable<TimeLogModel> GetAllFor(string connectionString, DateTime date, Guid accountId)
        {
            var repo = new TimeLogRepository(connectionString);
            var timeLogs = repo.GetAllFor(date, accountId);
            var timeLogModels = new List<TimeLogModel>();
            var adapter = new TimeLogAdapter();
            foreach(var log in timeLogs)
            {
                timeLogModels.Add(adapter.ToDomainObject(log, m__rosTimeFormat));
            }
            return timeLogModels.AsEnumerable();
        }

        public static TimeLogResponse UpdateTimeLog(string connectionString, TimeLogModel timeLog)
        {
            var repo = new TimeLogRepository(connectionString);
            if (null == timeLog.Id)
            {
                timeLog.Id = Guid.NewGuid();
            }
            var adapter = new TimeLogAdapter();
            var timeLogData = repo.UpdateTimeLog(adapter.ToBusinessObject(timeLog));
            timeLog.From = timeLogData.From.ToString(m__rosTimeFormat);
            timeLog.To = timeLogData.To?.ToString(m__rosTimeFormat);
            timeLog.Duration = (null == timeLogData.To) ? 0 : (int)(timeLogData.To.Value - timeLogData.From).TotalMinutes;
            return TimeLogResponseFactory.CreateFromModel(timeLog);
        }

        #endregion

    }
}