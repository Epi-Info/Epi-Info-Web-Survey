using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Common.DTO;
using System.Runtime.Serialization;

namespace Epi.Web.Common.Message
    {
      [DataContract(Namespace = "http://www.yourcompany.com/types/")]
   public class PreFilledAnswerResponse
        {
          public PreFilledAnswerResponse(PassCodeDTO DTO) 
              {
             

              this.SurveyResponsePassCode = DTO.PassCode;
              this.SurveyResponseID = DTO.ResponseId;
              }
          public PreFilledAnswerResponse()
              {
               
              }
          [DataMember]
          public Dictionary<string,string> ErrorMessageList;
          [DataMember]
          public string SurveyResponseUrl;
          [DataMember]
          public string Status;
          [DataMember]
          public string SurveyResponseID;
          [DataMember]
          public string SurveyResponsePassCode;

        }
    }
