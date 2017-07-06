using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeLogger.App.Core.Business;
using TimeLogger.App.Core.Repository;

namespace TimeLogger.App.Web.Code.Account
{
    public static class AccountUserService
    {

        #region Public methods

        public static AccountUser Create(string connectionString, Core.Business.AccountUser accountUser)
        {
            var repo = new AccountUserRepository(connectionString);
            return repo.Create(accountUser);
        }

        #endregion

    }
}