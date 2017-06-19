using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeLogger.App.Core.Business
{
    public class Billing
    {

        #region Properties

        public Guid Id { get; set; }
        public string PayProviderId { get; set; }
        public DateTime Created { get; set; }
        public DateTime? DatePaid { get; set; }
        public int BillingOptionId { get; set; }
        public Guid AccountId { get; set; }

        #endregion

    }
}
