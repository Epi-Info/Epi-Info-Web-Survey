using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Common.DTO;
using System.Runtime.Serialization;

namespace Epi.Web.Common.Message
    {
     [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class SurveyControlsResponse : Epi.Web.Common.MessageBase.ResponseBase 
        {
         [DataMember]
         public string Message;
         [DataMember]
         public List<SurveyControlDTO> SurveyControlList;

        }
    }
