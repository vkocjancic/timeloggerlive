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
    public class AccountUserRepository : AccountUserRepositoryBase
    {
        #region Constructors

        public AccountUserRepository(string connectionString) : base(connectionString)
        {
        }

        #endregion

        #region Overridden methods

        public override AccountUser Create(AccountUser accountUser)
        {
            accountUser.Id = Guid.NewGuid();
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                connection.Execute(@"insert into [ACCOUNT_USER]
                    ([ACCOUNT_USER_ID], [CREATED], [USER_ID], [ACCOUNT_ID])
                    values (@Id, @Created, @UserId, @AccountId)",
                    new
                    {
                        Id = accountUser.Id,
                        Created = accountUser.Created,
                        UserId = accountUser.UserId,
                        AccountId = accountUser.AccountId
                    });
            }
            return accountUser;
        }

        #endregion

    }
}
