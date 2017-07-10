using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeLogger.App.Core.Repository;

namespace TimeLogger.App.Web.Code.Assignment
{
    public static class AssignmentService
    {

        #region Public methods

        public static void ClearUnusedAssignmentsFor(string connectionString, Guid userId)
        {
            var repo = new AssignmentRepository(connectionString);
            repo.ClearUnusedAssignments(userId);
        }

        public static AssignmentModel GetOrCreateAssignment(string connectionString, string description, Guid userId)
        {
            var repo = new AssignmentRepository(connectionString);
            var assignment = AssignmentModelFactory.CreateFromBusinessModel(repo.GetByDescription(description, userId));
            if (null == assignment)
            {
                assignment = new AssignmentModel()
                {
                    Description = description,
                    Id = Guid.NewGuid(),
                    UserId = userId
                };
                repo.CreateAssignment(AssignmentModelFactory.ToBusinessObject(assignment));
            }
            return assignment;
        }

        public static IEnumerable<AssignmentModel> SearchAllFor(string connectionString, string query, Guid userId)
        {
            var repo = new AssignmentRepository(connectionString);
            var assignments = repo.SearchAllFor(query, userId);
            var assignmentModels = new List<AssignmentModel>();
            foreach (var assignment in assignments)
            {
                assignmentModels.Add(AssignmentModelFactory.CreateFromBusinessModel(assignment));
            }
            return assignmentModels;
        }

        #endregion

    }
}