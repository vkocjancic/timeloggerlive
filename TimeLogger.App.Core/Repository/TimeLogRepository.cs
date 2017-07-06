using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeLogger.App.Core.Business;

namespace TimeLogger.App.Core.Repository
{
    public class TimeLogRepository : TimeLogRepositoryBase
    {
        #region Constructors

        public TimeLogRepository(string connectionString) : base(connectionString)
        {
        }

        #endregion

        #region Overridden methods

        public override TimeLog CreateTimeLog(TimeLog timeLog)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                connection.Execute("insert into [TIME_LOG] (TIME_LOG_ID, [FROM], [TO], [DESCRIPTION], CREATED, ASSIGNMENT_ID, USER_ID)" +
                    " values (@Id, @From, @To, @Description, @Created, @AssignmentId, @UserId)",
                    new
                    {
                        Id = timeLog.Id,
                        From = timeLog.From,
                        To = timeLog.To,
                        Description = timeLog.Description,
                        Created = timeLog.Created,
                        AssignmentId = timeLog.AssignmentId,
                        UserId = timeLog.UserId
                    });
            }
            return timeLog;
        }

        public override void DeleteTimeLog(Guid id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                connection.Execute("delete from TIME_LOG where TIME_LOG_ID = @Id",
                    new
                    {
                        Id = id
                    });
            }
        }

        public override IEnumerable<TimeLog> GetAllFor(DateTime date, Guid accountId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                return connection.Query<TimeLog>(
                    @"select TIME_LOG_ID as Id, [FROM] as [From], [TO] as [To], [DESCRIPTION] as [Description], 
                      CREATED as Created, ASSIGNMENT_ID as AssignmentId, USER_ID as UserId
                      from [TIME_LOG] t
                      where t.[USER_ID] = @AccountId
                        and t.[FROM] between @CreatedStart and @CreatedEnd
                      order by t.[FROM]",
                    new
                    {
                        AccountId = accountId,
                        CreatedStart = date.Date,
                        CreatedEnd = date.Date.AddDays(1)
                    });
            }
        }

        public override TimeLog UpdateTimeLog(TimeLog timeLog)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                connection.Execute(
                    @"update [TIME_LOG]
                      set [FROM] = @From, [TO] = @To, [DESCRIPTION] = @Description
                      where TIME_LOG_ID=@Id" ,
                    new
                    {
                        Id = timeLog.Id,
                        From = timeLog.From,
                        To = timeLog.To,
                        Description = timeLog.Description
                    });
            }
            return timeLog;
        }

        #endregion

    }
}
