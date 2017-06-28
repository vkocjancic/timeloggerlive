using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeLogger.App.Core.Business;

namespace TimeLogger.App.Core.Repository
{
    public abstract class PasswordResetRepositoryBase : RepositoryBase
    {
        
        #region Constructors

        public PasswordResetRepositoryBase(string connectionString) : base(connectionString) { }

        #endregion

        #region Abstract methods

        public abstract void Create(Guid requestId, Guid userId);

        public abstract PasswordResetRequest GetById(Guid requestId, DateTime date);

        public abstract void Invalidate(Guid requestId);

        #endregion

    }
}
