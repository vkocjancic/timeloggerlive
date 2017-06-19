using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeLogger.App.Core.Business;

namespace TimeLogger.App.Core.Repository
{
    public abstract class AccountRepositoryBase : RepositoryBase
    {
        #region Constructors

        public AccountRepositoryBase(string connectionString) : base(connectionString)
        {
        }

        #endregion

        #region Abstract methods

        public abstract Account CreateAccount(Guid userId, bool isActive);

        #endregion

    }
}
