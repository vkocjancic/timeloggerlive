using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeLogger.App.Core.Repository
{
    public abstract class RepositoryBase
    {

        #region Properties

        protected string ConnectionString { get; set; }

        #endregion

        #region Constructors

        public RepositoryBase(string connectionString)
        {
            ConnectionString = connectionString;
        }

        #endregion

    }
}
