using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeLogger.App.Web.Code.Admin
{
    public class DashboardModel
    {

        #region Properties

        [JsonProperty(PropertyName = "accountsall")]
        public int AccountsAll { get; set; }

        [JsonProperty(PropertyName = "accountsactive")]
        public int AccountsActive { get; set; }

        #endregion

        #region Methods

        public int GetActiveAccountsPercentage()
        {
            return (int)(((decimal)AccountsActive / (decimal)AccountsAll) * 100M);
        }

        #endregion

    }
}