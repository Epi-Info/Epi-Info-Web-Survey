using Epi.Web.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Epi.Web.Common.Message
{
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class DashboardResponse : Epi.Web.Common.MessageBase.ResponseBase
    {

        public DashboardResponse()
        {
            this.SurveyInfo = new SurveyInfoDTO();
          
        }
        [DataMember]
        public SurveyInfoDTO SurveyInfo;
        [DataMember]
        public int DownloadedRecordCount { get; set; }

        [DataMember]
        public int RecordCount { get; set; }
        [DataMember]
        public int SubmitedRecordCount { get; set; }
        [DataMember]
        public int StartedRecordCount { get; set; }
        [DataMember]
        public int SavedRecordCount { get; set; }
        [DataMember]
        public Dictionary<string,int> RecordCountPerDate { get; set; }

        [DataMember]
        public List<string>  DateList { get; set; }
    }
}
