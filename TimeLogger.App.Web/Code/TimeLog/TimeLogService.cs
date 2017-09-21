using NLog;
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
        private static Logger m_log = LogManager.GetLogger(typeof(TimeLogService).FullName);

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
            m_log.Info($"'{accountId}' GetAllFor method invoked for date '{date}'");
            var repo = new TimeLogRepository(connectionString);
            m_log.Debug($"'{accountId}' Obtaining time logs");
            var timeLogs = repo.GetAllFor(date, accountId);
            m_log.Info($"'{accountId}' {timeLogs.Count()} time log(s) found");
            var timeLogModels = new List<TimeLogModel>();
            var adapter = new TimeLogAdapter();
            foreach(var log in timeLogs)
            {
                timeLogModels.Add(adapter.ToDomainObject(log, m__rosTimeFormat));
            }
            m_log.Info($"'{accountId}' Time logs converted to domain object array");
            return timeLogModels.AsEnumerable();
        }

        public static IEnumerable<TimeLogModel> GetAllForTask(string connectionString, Guid taskId, Guid userId)
        {
            var repo = new TimeLogRepository(connectionString);
            var timeLogs = repo.GetAllForTask(taskId, userId);
            var timeLogModels = new List<TimeLogModel>();
            var adapter = new TimeLogAdapter();
            foreach (var log in timeLogs)
            {
                timeLogModels.Add(adapter.ToDomainObject(log, "H:mm"));
            }
            return timeLogModels;
        }

        public static void UpdateAllForTask(string connectionString, Guid taskId, Guid userId, Action<Core.Business.TimeLog> updateTimeLog)
        {
            var repo = new TimeLogRepository(connectionString);
            var timeLogs = repo.GetAllForTask(taskId, userId);
            foreach(var log in timeLogs)
            {
                updateTimeLog(log);
                repo.UpdateTimeLog(log);
            }
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