using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeLogger.App.Core.Repository;

namespace TimeLogger.App.Web.Code.Billing
{
    public static class BillingService
    {

        #region Public methods

        public static void CreateBilling(string connectionString, AccountModel account, string plan)
        {
            if (null != account)
            {
                var repo = new BillingRepository(connectionString);
                var billingOption = repo.GetBillingOptionByAccountTypeCode(plan);
                repo.CreateBilling(account.Id, billingOption, "PAYPAL");
            }
        }

        #endregion

    }
}