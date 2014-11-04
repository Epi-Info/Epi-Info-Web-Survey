using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Epi.Web.Enter.Common.DTO
    {
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class PreFilledAnswerDTO
        {
        
        [DataMember]
        public Guid SurveyId { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public Guid ParentRecordId { get; set; }
        [DataMember]
        public Guid ResponseId { get; set; }
        [DataMember]
        public Dictionary<string,string> SurveyQuestionAnswerList { get; set; }
         [DataMember]
        public Guid OrganizationKey { get; set; }
        
 
        }
    }
