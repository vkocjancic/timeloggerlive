using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeLogger.App.Core.Business;

namespace TimeLogger.App.Core.Repository
{
    public abstract class DashboardRepositoryBase : RepositoryBase
    {

        #region Constructors

        public DashboardRepositoryBase(string connectionString) : base(connectionString)
        {
        }

        #endregion

        #region Abstract methods

        public abstract Dashboard GetData();

        #endregion

    }
}
