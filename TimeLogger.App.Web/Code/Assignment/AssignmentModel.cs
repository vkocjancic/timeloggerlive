using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeLogger.App.Web.Code.Assignment
{
    public class AssignmentModel
    {

        #region Property

        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "descriptionold")]
        public string DescriptionOld { get; set; }

        [JsonIgnore]
        public Guid? UserId { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "isfavourite")]
        public bool IsFavourite { get; set; }

        [JsonProperty(PropertyName = "wasmerged")]
        public bool WasMerged { get; set; }

        #endregion

    }
}