using System.Runtime.Serialization;
using System.Collections.Generic;
using Epi.Web.Enter.Common.MessageBase;
using Epi.Web.Enter.Common.Criteria;
using Epi.Web.Enter.Common.DTO;

namespace Epi.Web.Enter.Common.Message
    {
    /// <summary>
    /// Represents a SurveyInfo request message from client.
    /// </summary>
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]

    public class FormsInfoRequest
        {
         public FormsInfoRequest()
        {
            this.Criteria = new FormInfoCriteria();
             
        }

        /// <summary>
        /// Selection criteria and sort order
        /// </summary>
        [DataMember]
        public FormInfoCriteria Criteria;


       
        }
    }
