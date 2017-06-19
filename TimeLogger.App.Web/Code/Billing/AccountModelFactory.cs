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
                Id = account.Id
            };
        }

        #endregion

    }
}