using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Epi.Web.Common.Message
{
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class PublishReportRequest : Epi.Web.Common.MessageBase.RequestBase
    {
        public PublishReportRequest()
        {

            this.ReportInfo = new DTO.ReportInfoDTO();

        }

        [DataMember]
        public DTO.ReportInfoDTO ReportInfo;
        [DataMember]
        public bool IncludHTML { get; set; }
    }
}
