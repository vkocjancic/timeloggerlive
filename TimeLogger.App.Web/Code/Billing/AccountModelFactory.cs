using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeLogger.App.Core.Business;

namespace TimeLogger.App.Web.Code.Billing
{
    public static class AccountModelFactory
    {

        #region Public methods

        internal static AccountModel CreateFromBusinessModel(Core.Business.Account account)
        {
            return new AccountModel()
            {
                Id = account.Id,
                BillingOption = account.BillingOption,
                Created = account.Created,
                Description = account.Description,
                Email = account.Email,
                IsActive = account.IsActive,
                Price = account.Price
            };
        }

        internal static Core.Business.Account ToBusinessObject(AccountModel account)
        {
            return new Core.Business.Account()
            {
                Id = account.Id,
                Created = account.Created,
                Description = account.Description,
                IsActive = account.IsActive,
            };
        }

        #endregion

    }
}