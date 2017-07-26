using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeLogger.App.Core.Business;
using TimeLogger.App.Core.Insights;

namespace TimeLogger.App.Core.Repository
{
    public class InsightsChartRepository : InsightsChartRepositoryBase
    {

        #region Constructors

        public InsightsChartRepository(string connectionString) : base(connectionString)
        {
        }

        #endregion

        #region Overridden methods

        public override IEnumerable<InsightsChartData> GetData(InsightsChart model)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                if (InsightsInterval.Year == model.Interval)
                {
                    return connection.Query<InsightsChartData>(
                        @"select Label, COUNT([Description]) as Tasks, Sum([Hours]) as [Hours]
                            from (
	                            select DATEADD(MONTH, DATEDIFF(MONTH, 0, [FROM]), 0) as Label, [DESCRIPTION], SUM(DATEDIFF(mi, [FROM], [TO])) / 60.0 as Hours
                                from TIME_LOG
                                where [USER_ID] = @UserId and [FROM] >= @StartDate and [TO] is not null and [TO] <= DATEADD(d, 1, @EndDate)
                                group by DATEADD(MONTH, DATEDIFF(MONTH, 0, [FROM]), 0), [DESCRIPTION]
                            ) as foo
                            group by Label",
                        new { StartDate = model.StartDate, EndDate = model.EndDate, UserId = model.UserId });
                }
                else
                {
                    return connection.Query<InsightsChartData>(
                        @"select Label, COUNT(Description) as Tasks, Sum(Hours) as Hours
                            from (
	                            select cast([FROM] as date) as Label, [DESCRIPTION], SUM(DATEDIFF(mi, [FROM], [TO])) / 60.0 as Hours
	                            from TIME_LOG
                                where [USER_ID] = @UserId and [FROM] >= @StartDate and [TO] is not null and [TO] <= DATEADD(d, 1, @EndDate)
                                group by cast([FROM] as date), [DESCRIPTION]
                            ) as logs
                            group by Label",
                        new { StartDate = model.StartDate, EndDate = model.EndDate, UserId = model.UserId });
                }
            }
        }

        #endregion

    }
}
