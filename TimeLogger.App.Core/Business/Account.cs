using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeLogger.App.Core.Business
{
    public class Account
    {

        #region Properties

        public Guid Id { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public Guid OwnerUserId { get; set; }
        public bool IsActive { get; set; }

        #endregion

        #region Extended properties

        public string Email { get; set; }
        public string BillingOption { get; set; }
        public decimal Price { get; set; }

        #endregion

    }
}
