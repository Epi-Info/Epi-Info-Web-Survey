using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Epi.Web.Common.DTO
{
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class SurveyResponseDTO
    {
        [DataMember]
        public string ResponseId{ get; set; }
        [DataMember]
        public string SurveyId { get; set; }
        [DataMember]
        public DateTime DateLastUpdated { get; set; }
        [DataMember]
        public DateTime DateCompleted { get; set; }
        [DataMember]
        public int Status { get; set; }
        [DataMember]
        public string XML { get; set; }
    }
}
