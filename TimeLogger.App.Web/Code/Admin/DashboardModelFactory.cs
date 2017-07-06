using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeLogger.App.Core.Business;

namespace TimeLogger.App.Web.Code.Admin
{
    public static class DashboardModelFactory
    {

        public static DashboardModel CreateFromBusinessObject(Dashboard data)
        {
            return new DashboardModel()
            {
                AccountsActive = data.AccountsActive,
                AccountsAll = data.AccountsAll
            };
        }

    }
}