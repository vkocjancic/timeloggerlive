using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeLogger.App.Core.Business
{
    public class BillingOption
    {

        #region Properties

        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Payments { get; set; }
        public decimal Price { get; set; }
        public DateTime Created { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public int AccountTypeId { get; set; }

        #endregion

    }
}
