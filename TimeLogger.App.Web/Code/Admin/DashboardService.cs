using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeLogger.App.Core.Repository;

namespace TimeLogger.App.Web.Code.Admin
{

    public static class DashboardService
    {

        #region Public methods

        public static DashboardModel GetData(string connectionString)
        {
            var repo = new DashboardRepository(connectionString);
            return DashboardModelFactory.CreateFromBusinessObject(repo.GetData());
        }

        #endregion

    }

}