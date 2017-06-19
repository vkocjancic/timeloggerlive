using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeLogger.App.Core.Business;

namespace TimeLogger.App.Core.Repository
{
    public abstract class UserRepositoryBase : RepositoryBase
    {

        #region Constructors

        public UserRepositoryBase(string connectionString) : base(connectionString) { }

        #endregion

        #region Abstract mehtods

        public abstract User GetByEmail(string email);

        public abstract User CreateUser(User user);
                
        #endregion

    }
}
