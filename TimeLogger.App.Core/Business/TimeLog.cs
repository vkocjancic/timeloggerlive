using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeLogger.App.Core.Business
{
    public class TimeLog
    {

        #region Properties

        public Guid Id { get; set; }
        public DateTime From { get; set; }
        public DateTime? To { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public Guid? AssignmentId { get; set; }
        public Guid? UserId { get; set; }

        #endregion

    }
}
