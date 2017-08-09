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



        public override IEnumerable<Account> GetAllInactive()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                return connection.Query<Account>(@"
                    select a.[ACCOUNT_ID] as Id, case when a.[ACTIVE_YN] = 'Y' then 1 else 0 end as IsActive, a.[CREATED] as Created, a.[DESCRIPTION] as Description, a.[OWNER_USER_ID] as OwnerUserId,
	                    u.[EMAIL] as Email, 
	                    bo.[DESCRIPTION] as BillingOption, bo.[PRICE] as Price
                    from [ACCOUNT] a
                    join [USER] u on a.[OWNER_USER_ID] = u.[USER_ID]
                    join [BILLING] b on a.[ACCOUNT_ID] = b.[ACCOUNT_ID]
                    join [CD_BILLING_OPTION] bo on b.[BILLING_OPTION_ID] = bo.[BILLING_OPTION_ID]
                    where a.[ACTIVE_YN] = 'N'");
            }
        }

        public override Account GetById(Guid accountId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                return connection.Query<Account>(@"
                    select a.[ACCOUNT_ID] as Id, case when a.[ACTIVE_YN] = 'Y' then 1 else 0 end as IsActive, a.[CREATED] as Created, a.[DESCRIPTION] as Description, a.[OWNER_USER_ID] as OwnerUserId
                    from [ACCOUNT] a
                    where a.[ACCOUNT_ID]=@Id",
                    new { Id = accountId }).FirstOrDefault();
            }
        }

        public override Account UpdateAccount(Account account)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                connection.Execute(
                    @"update [ACCOUNT]
                      set [ACTIVE_YN]=@Active
                      where ACCOUNT_ID=@Id",
                    new
                    {
                        Id = account.Id,
                        Active = account.IsActive ? 'Y' : 'N'
                    });
            }
            return account;
        }

        #endregion

    }
}
