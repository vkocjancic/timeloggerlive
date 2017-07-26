using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeLogger.App.Core.Business;

namespace TimeLogger.App.Core.Repository
{
    public abstract class TimeLogRepositoryBase : RepositoryBase
    {

        #region Constructors

        public TimeLogRepositoryBase(string connectionString) : base(connectionString)
        {
        }

        #endregion

        #region Abstact methods

        public abstract TimeLog CreateTimeLog(TimeLog timeLog);

        public abstract void DeleteTimeLog(Guid id);

        public abstract IEnumerable<TimeLog> GetAllFor(DateTime date, Guid accountId);

        public abstract IEnumerable<TimeLog> GetAllFor(DateTime startDate, DateTime endDate, Guid accountId);

        public abstract TimeLog UpdateTimeLog(TimeLog timeLog);

        #endregion

    }
}
