using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeLogger.App.Core.Business;

namespace TimeLogger.App.Web.Code.Account
{
    public static class AccountUserFactory
    {
        internal static AccountUser CreateFromAccountBusinessObject(Core.Business.Account accountData)
        {
            return new AccountUser()
            {
                AccountId = accountData.Id,
                Created = DateTime.Now,
                UserId = accountData.OwnerUserId
            };
        }
    }
}