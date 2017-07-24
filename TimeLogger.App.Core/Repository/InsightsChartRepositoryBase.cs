using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeLogger.App.Core.Business;

namespace TimeLogger.App.Core.Repository
{
    public abstract class InsightsChartRepositoryBase : RepositoryBase
    {
        
        #region Constructors

        public InsightsChartRepositoryBase(string connectionString) : base(connectionString)
        {
        }

        #endregion

        #region Abstract methods

        public abstract IEnumerable<InsightsChartData> GetData(InsightsChart model);

        #endregion

    }
}
