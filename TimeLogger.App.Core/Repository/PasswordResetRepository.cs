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
    public class PasswordResetRepository : PasswordResetRepositoryBase
    {
        #region Constructors

        public PasswordResetRepository(string connectionString) : base(connectionString) { }

        #endregion
        
        #region Overrides

        public override void Create(Guid requestId, Guid userId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                connection.Execute(@"insert into PASSWORD_RESET_REQUEST ([PASSWORD_RESET_REQUEST_ID], [CREATED], [ACTIVE_YN], [USER_ID])
                                     values (@RequestId, @Created, @ActiveYN, @UserId)",
                                     new { RequestId = requestId, Created = DateTime.Now, ActiveYN = 'Y', UserId = userId });
            }
        }

        public override PasswordResetRequest GetById(Guid requestId, DateTime date)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                return connection.Query<PasswordResetRequest>(
                    @"select [PASSWORD_RESET_REQUEST_ID] as Id, [CREATED] as Created, 
                            case when [ACTIVE_YN] = 'Y' then 1 else 0 end as IsActive, [USER_ID] as UserId
                        from PASSWORD_RESET_REQUEST
                        where PASSWORD_RESET_REQUEST_ID = @RequestId
                            and CREATED >= @Date",
                    new { RequestId = requestId, Date = date }).FirstOrDefault();
            }
        }

        public override void Invalidate(Guid requestId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                connection.Execute(@"update PASSWORD_RESET_REQUEST
                                     set [ACTIVE_YN] = 'N'
                                     where PASSWORD_RESET_REQUEST_ID = @RequestId",
                                     new { RequestId = requestId });
            }
        }

        #endregion

    }
}
