using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeLogger.App.Web.Code.Billing
{
    public static class AccountTypeFactory
    {

        #region Public methods

        public static AccountType FromString(string accountType)
        {
            switch (accountType?.ToUpper()) {
                case "BUSINESS":
                    return AccountType.Business;
                case "PERSONAL":
                    return AccountType.Personal;
                default:
                    return AccountType.Personal;
            }
        }

        #endregion

    }
}