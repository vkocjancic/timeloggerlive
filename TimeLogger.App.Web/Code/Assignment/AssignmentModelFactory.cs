using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeLogger.App.Core.Business;

namespace TimeLogger.App.Web.Code.Assignment
{
    public static class AssignmentModelFactory
    {

        #region Public methods

        public static AssignmentModel CreateFromBusinessModel(Core.Business.Assignment assignment)
        {
            if (null == assignment)
            {
                return null;
            }
            return new AssignmentModel()
            {
                Description = assignment.Description,
                Id = assignment.Id,
                IsFavourite = assignment.IsFavourite,
                Status = assignment.Status
            };
        }

        public static Core.Business.Assignment ToBusinessObject(AssignmentModel assignment)
        {
            return new Core.Business.Assignment()
            {
                Created = DateTime.Now,
                Description = assignment.Description,
                Id = assignment.Id,
                IsFavourite = assignment.IsFavourite,
                UserId = assignment.UserId,
                Status = assignment.Status
            };
        }

        #endregion

    }
}