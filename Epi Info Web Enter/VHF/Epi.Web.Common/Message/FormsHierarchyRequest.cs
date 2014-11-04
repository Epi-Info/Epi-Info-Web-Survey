using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Enter.Common.DTO;
using System.Runtime.Serialization;
namespace Epi.Web.Enter.Common.Message
    {
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class FormsHierarchyRequest : Epi.Web.Enter.Common.MessageBase.RequestBase
        {
        public FormsHierarchyRequest()
        {
        this.SurveyInfo = new FormInfoDTO();
        this.SurveyResponseInfo = new FormResponseInfoDTO();
        }

        /// <summary>
        /// SurveyInfo object.
        /// </summary>
        [DataMember]
        public FormInfoDTO SurveyInfo;

        [DataMember]
        public FormResponseInfoDTO SurveyResponseInfo;
        }
    }
