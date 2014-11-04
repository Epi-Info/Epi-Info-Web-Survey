using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;


namespace Epi.Web.Enter.Common.DTO
    {
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class FormResponseInfoDTO
        {
        [DataMember]
        public string ResponseId { get; set; }
        [DataMember]
        public string SurveyId { get; set; }
        [DataMember]
        public DateTime DateUpdated { get; set; }
        [DataMember]
        public DateTime? DateCompleted { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public int Status { get; set; }
        [DataMember]
        public string XML { get; set; }
        [DataMember]
        public Guid UserPublishKey { get; set; }

        [DataMember]
        public bool IsDraftMode { get; set; }
        }
    }
