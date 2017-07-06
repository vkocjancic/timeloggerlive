using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeLogger.App.Core.Business;

namespace TimeLogger.App.Core.Repository
{
    public abstract class AccountUserRepositoryBase : RepositoryBase
    {
        #region Constructors

        public AccountUserRepositoryBase(string connectionString) : base(connectionString)
        {
        }

        #endregion

        #region Abstract methods

        public abstract AccountUser Create(AccountUser accountUser);

        #endregion

    }
}
