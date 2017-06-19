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
    public class BillingRepository : BillingRepositoryBase
    {
        
        #region Constructors

        public BillingRepository(string connectionString) : base(connectionString)
        {
        }

        #endregion
        
        #region Overridden methods

        public override Billing CreateBilling(Guid accountId, BillingOption option, string providerName)
        {
            var billing = new Billing()
            {
                Id = Guid.NewGuid(),
                PayProviderId = providerName,
                Created = DateTime.Now,
                BillingOptionId = option.Id,
                AccountId = accountId
            };
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                connection.Execute("insert into [BILLING] (BILLING_ID, PAY_PROVIDER_ID, CREATED, BILLING_OPTION_ID, ACCOUNT_ID)" +
                    " values (@Id, @PayProvider, @DateCreated, @BillingOptionId, @AccountId)",
                    new
                    {
                        Id = billing.Id,
                        PayProvider = billing.PayProviderId,
                        DateCreated = billing.Created,
                        BillingOptionId = billing.BillingOptionId,
                        AccountId = billing.AccountId
                    });
            }
            return billing;
        }

        public override BillingOption GetBillingOptionByAccountTypeCode(string code)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                return connection.Query<BillingOption>(
                    @"select bo.BILLING_OPTION_ID as Id, bo.CODE as Code, bo.DESCRIPTION as Description, bo.PAYMENTS_MY as Payments,
                        bo.PRICE as Price, bo.CREATED as Created, bo.VALID_FROM as ValidFrom, bo.VALID_TO as ValidTo, bo.ACCOUNT_TYPE_ID as AccountTypeId
                      from [CD_BILLING_OPTION] bo
                      join [CD_ACCOUNT_TYPE] at on bo.ACCOUNT_TYPE_ID = at.ACCOUNT_TYPE_ID
                      where at.CODE = @CODE 
                        and bo.VALID_FROM <= getdate() 
                        and (bo.VALID_TO is null or bo.VALID_TO >= getdate())",
                        new { CODE = code }).FirstOrDefault();
            }
        } 

        #endregion

    }
}
