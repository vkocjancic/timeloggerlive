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
    public class DashboardRepository : DashboardRepositoryBase
    {
        #region Constructors

        public DashboardRepository(string connectionString) : base(connectionString)
        {
        }

        #endregion

        #region Overridden methods

        public override Dashboard GetData()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                return connection.Query<Dashboard>(
                    @"select COUNT([ACCOUNT_ID]) as AccountsAll, SUM(case when [ACTIVE_YN]='Y' then 1 else 0 end) as AccountsActive
                        from [ACCOUNT]").FirstOrDefault();
            }
        }

        #endregion

    }
}
