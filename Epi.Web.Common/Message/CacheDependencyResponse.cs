using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Epi.Web.Common.MessageBase;
using Epi.Web.Common.DTO;

namespace Epi.Web.Common.Message
{
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class CacheDependencyResponse : ResponseBase
    {
        [DataMember]
        public Dictionary<string, System.DateTime> SurveyDependency;

        public CacheDependencyResponse() 
        {
            SurveyDependency = new Dictionary<string, System.DateTime>(); 
        }
    }
}
