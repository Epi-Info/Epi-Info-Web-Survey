using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Common.DTO;
using System.Runtime.Serialization;
namespace Epi.Web.Common.Message
    {
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class FormsHierarchyRequest : Epi.Web.Common.MessageBase.RequestBase
        {
        public FormsHierarchyRequest()
        {
            this.SurveyInfo = new SurveyInfoDTO();//new FormInfoDTO();
            this.SurveyResponseInfo = new SurveyAnswerDTO();//new FormResponseInfoDTO();
        }

        /// <summary>
        /// SurveyInfo object.
        /// </summary>
        [DataMember]
        ///public FormInfoDTO SurveyInfo;
        public SurveyInfoDTO SurveyInfo;
        [DataMember]
        // public FormResponseInfoDTO SurveyResponseInfo;
        public SurveyAnswerDTO SurveyResponseInfo;
    }
    }
