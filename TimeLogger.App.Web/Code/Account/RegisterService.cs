using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Security;
using TimeLogger.App.Core.Repository;

namespace TimeLogger.App.Web.Code.Account
{
    public static class RegisterService
    {

        #region Public methods

        public static RegisterResponse CreateUser(UserModel user)
        {
            MembershipCreateStatus createStatus;
            var newUser = Membership.CreateUser(user.Email, user.Password, user.Email, "?", "!", false, out createStatus);
            return RegisterResponseFactory.CreateFromMembershipStatus(createStatus);
        }

        public static Billing.AccountModel CreateAccount(string connectionString, UserModel userModel)
        {
            var user = Membership.GetUser(userModel.Email);
            if (null != user) {
                var repo = new AccountRepository(connectionString);
                return Billing.AccountModelFactory.CreateFromBusinessModel(repo.CreateAccount((Guid)user.ProviderUserKey, false));
            }
            return null;
        }

        #endregion

    }
}