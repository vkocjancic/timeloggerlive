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
    public class AccountRepository : AccountRepositoryBase
    {
        
        #region Constructors

        public AccountRepository(string connectionString) : base(connectionString)
        {
        }

        #endregion

        #region Overridden methods
        
        public override Account CreateAccount(Guid userId, bool isActive)
        {
            var account = new Account()
            {
                Id = Guid.NewGuid(),
                Created = DateTime.Now,
                OwnerUserId = userId,
                IsActive = isActive
            };
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                connection.Execute("insert into [ACCOUNT] (ACCOUNT_ID, DESCRIPTION, CREATED, OWNER_USER_ID, ACTIVE_YN)" +
                    " values (@Id, @Description, @DateCreated, @OwnerUserId, @Active)",
                    new {
                        Id = account.Id,
                        Description = account.Description,
                        DateCreated = account.Created,
                        OwnerUserId = account.OwnerUserId,
                        Active = (account.IsActive) ? 'Y' : 'N'
                    });
            }
            return account;
        }

        #endregion

    }
}
