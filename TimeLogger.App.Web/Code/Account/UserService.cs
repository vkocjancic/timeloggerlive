using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeLogger.App.Core.Repository;

namespace TimeLogger.App.Web.Code.Account
{
    public static class UserService
    {

        #region Public methods

        public static void ActivateUser(string connectionString, Guid ownerUserId)
        {
            var repo = new UserRepository(connectionString);
            repo.ActivateUser(ownerUserId);
        }

        #endregion

    }
}