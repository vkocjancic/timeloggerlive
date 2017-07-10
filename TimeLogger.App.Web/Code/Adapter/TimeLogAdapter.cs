using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeLogger.App.Web.Code.TimeLog;

namespace TimeLogger.App.Web.Code.Adapter
{
    public class TimeLogAdapter
    {

        #region Public methods

        public Core.Business.TimeLog ToBusinessObject(TimeLogModel model)
        {
            return new Core.Business.TimeLog()
            {
                AssignmentId = model.AssignmentId,
                Id = model.Id.Value,
                Created = DateTime.Now,
                Description = model.Description,
                From = model.GetFrom(),
                To = model.GetTo(),
                UserId = model.AccountId
            };
        }

        public TimeLogModel ToDomainObject(Core.Business.TimeLog log, string timeFormat)
        {
            return new TimeLogModel()
            {
                AccountId = log.UserId.Value,
                AssignmentId = log.AssignmentId,
                Date = log.Created.ToString("d"),
                Description = log.Description,
                Duration = (null == log.To) ? 0 : (int)(log.To.Value - log.From).TotalMinutes,
                From = log.From.ToString(timeFormat),
                Id = log.Id,
                To = log.To?.ToString(timeFormat)
            };
        }

        #endregion

    }
}