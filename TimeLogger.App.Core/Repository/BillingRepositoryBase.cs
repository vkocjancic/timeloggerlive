using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeLogger.App.Core.Business;

namespace TimeLogger.App.Core.Repository
{
    public abstract class BillingRepositoryBase : RepositoryBase
    {
        
        #region Constructors

        public BillingRepositoryBase(string connectionString) : base(connectionString)
        {
        }

        #endregion

        #region Abstract methods

        public abstract BillingOption GetBillingOptionByAccountTypeCode(string code);

        public abstract Billing CreateBilling(Guid accountId, BillingOption option, string providerName);

        #endregion

    }
}
