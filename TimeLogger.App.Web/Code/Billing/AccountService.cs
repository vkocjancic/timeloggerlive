using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeLogger.App.Core.Repository;
using TimeLogger.App.Web.Code.Account;

namespace TimeLogger.App.Web.Code.Billing
{
    public static class AccountService
    {

        #region Public methods

        public static IEnumerable<AccountModel> GetAllFor(string connectionString, string accountType)
        {
            var repo = new AccountRepository(connectionString);
            var accountModels = new List<AccountModel>();
            foreach (var account in repo.GetAllInactive())
            {
                accountModels.Add(AccountModelFactory.CreateFromBusinessModel(account));
            }
            return accountModels.AsEnumerable();
        }

        public static AccountListResponse ActivateAccount(string connectionString, AccountModel model)
        {
            var repo = new AccountRepository(connectionString);
            if (null == model.Id)
            {
                model.Id = Guid.NewGuid();
            }
            repo.UpdateAccount(AccountModelFactory.ToBusinessObject(model));
            var accountData = repo.GetById(model.Id);
            UserService.ActivateUser(connectionString, accountData.OwnerUserId);
            AccountUserService.Create(connectionString, AccountUserFactory.CreateFromAccountBusinessObject(accountData));
            return new AccountListResponse()
            {
                Account = AccountModelFactory.CreateFromBusinessModel(accountData)
            };
        }

        #endregion

    }
}