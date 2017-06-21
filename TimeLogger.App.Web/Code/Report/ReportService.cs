using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeLogger.App.Core.Repository;
using TimeLogger.App.Web.Code.Adapter;
using TimeLogger.App.Web.Code.TimeLog;

namespace TimeLogger.App.Web.Code.Report
{
    public static class ReportService
    {

        #region Public methods

        public static IEnumerable<ReportModel> GetAllFor(string connectionString, DateTime date, Guid accountId)
        {
            var repo = new TimeLogRepository(connectionString);
            var timeLogs = repo.GetAllFor(date, accountId);
            var timeLogModels = new List<TimeLogModel>();
            var adapter = new TimeLogAdapter();
            foreach (var log in timeLogs)
            {
                timeLogModels.Add(adapter.ToDomainObject(log, "H:mm"));
            }
            return timeLogModels
                .Where(tl => !string.IsNullOrEmpty(tl.To))
                .GroupBy(x => x.Description, StringComparer.InvariantCultureIgnoreCase)
                .Select(g => new ReportModel() { Title = g.Key, Duration = g.Sum(s => s.Duration) });
        }

        #endregion

    }
}