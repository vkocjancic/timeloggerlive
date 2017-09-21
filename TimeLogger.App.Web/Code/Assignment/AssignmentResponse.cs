using Newtonsoft.Json;
using TimeLogger.App.Web.Code.Common;
using TimeLogger.App.Web.Code.TimeLog;

namespace TimeLogger.App.Web.Code.Assignment
{

    public class AssignmentResponse : ApiResponse
    {

        #region Properties

        [JsonProperty(PropertyName = "timelogs")]
        public TimeLogModel[] TimeLogs { get; set; }

        [JsonProperty(PropertyName = "wasmerged")]
        public bool WasMerged { get; set; }

        #endregion

    }

}