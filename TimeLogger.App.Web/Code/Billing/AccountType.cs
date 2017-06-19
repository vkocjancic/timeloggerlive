using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeLogger.App.Web.Code.Billing
{

    public abstract class AccountType
    {

        #region Fields

        public static AccountType Personal = new AccountTypePersonal();
        public static AccountType Business = new AccountTypeBusiness();

        #endregion

        #region Constructors

        private AccountType() { }

        #endregion

        #region Abstract methods

        public abstract string GetCode();

        public abstract int GetPrice();

        #endregion

        #region Virtual methods

        public virtual string GeneratePayPalLink()
        {
            return $"https://paypal.me/vkocjancic/{this.GetPrice()}";
        }

        #endregion

        #region Personal

        private class AccountTypePersonal : AccountType
        {

            #region AccountType implementation

            public override string GetCode()
            {
                return "PRSN";
            }

            public override int GetPrice()
            {
                return 10;
            }

            #endregion

        }

        #endregion

        #region Business

        private class AccountTypeBusiness : AccountType
        {

            #region AccountType implementation

            public override string GetCode()
            {
                return "BSNS";
            }

            public override int GetPrice()
            {
                return 100;
            }

            #endregion

        }

        #endregion

    }

}