using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeLogger.App.Core.Business;

namespace TimeLogger.App.Core.Repository
{
    public abstract class AssignmentRepositoryBase : RepositoryBase
    {
        
        #region Constructors

        public AssignmentRepositoryBase(string connectionString) : base(connectionString)
        {
        }

        #endregion

        #region Abstract methods

        public abstract void ClearUnusedAssignments(Guid userId);

        public abstract void CreateAssignment(Assignment assignment);

        public abstract IEnumerable<Assignment> GetAllFor(Guid userId);

        public abstract Assignment GetByDescription(string description, Guid userId);

        public abstract IEnumerable<Assignment> SearchAllFor(string query, Guid userId);

        #endregion

    }
}
