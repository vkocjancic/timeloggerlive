using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeLogger.App.Core.Business
{
    public class PasswordResetRequest
    {

        #region Properties

        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public bool IsActive { get; set; }
        public Guid UserId { get; set; }

        #endregion

    }
}
