using System.Runtime.Serialization;
using System.Collections.Generic;
using Epi.Web.Enter.Common.MessageBase;
using Epi.Web.Enter.Common.Criteria;
using Epi.Web.Enter.Common.DTO;

namespace Epi.Web.Enter.Common.Message
    {
     [DataContract(Namespace = "http://www.yourcompany.com/types/")]
   public class FormSettingResponse
        {

         public FormSettingResponse()
        {
        this.FormSetting = new FormSettingDTO();
        this.FormInfo = new FormInfoDTO();
        }
         [DataMember]
         public FormSettingDTO FormSetting;

         [DataMember]
         public FormInfoDTO FormInfo;
        }
    }
