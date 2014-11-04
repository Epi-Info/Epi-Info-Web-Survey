using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Enter.Common.DTO;
using System.Runtime.Serialization;

namespace Epi.Web.Enter.Common.Message
{
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class PublishRequest : Epi.Web.Enter.Common.MessageBase.RequestBase
    {

        public PublishRequest()
        {
            this.SurveyInfo = new SurveyInfoDTO();
        }

        /// <summary>
        /// SurveyInfo object.
        /// </summary>
        [DataMember]
        public SurveyInfoDTO SurveyInfo;
    }
}
