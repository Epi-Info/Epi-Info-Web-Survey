using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Epi.Web.Common
{
   public class SurveyDashboardBO
    {
        [DataMember]
        public int RecordCount { get; set; }
        [DataMember]
        public int SubmitedRecordCount { get; set; }
        [DataMember]
        public int StartedRecordCount { get; set; }
        [DataMember]
        public int SavedRecordCount { get; set; }
        [DataMember]
        public Dictionary<string, int> RecordCountPerDate { get; set; }
    }
}
    