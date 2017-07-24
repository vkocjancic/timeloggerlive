using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeLogger.App.Core.Insights;

namespace TimeLogger.App.Core.Business
{
    public class InsightsChart
    {
        
        #region Properties

        public DateTime EndDate { get; set; }
        public InsightsInterval Interval { get; set; }
        public DateTime StartDate { get; set; }
        public Guid UserId { get; set; } 

        #endregion

    }
}
