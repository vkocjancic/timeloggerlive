using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeLogger.App.Web.Code.Common;
using TimeLogger.App.Web.Code.TimeLog;

namespace TimeLogger.App.Web.Code.Assignment
{

    public class AssignmentCollectionResponse : ApiResponse
    {

        #region Properties

        [JsonProperty(PropertyName = "assignments")]
        public AssignmentModel[] Assignments { get; set; }

        #endregion

    }

}