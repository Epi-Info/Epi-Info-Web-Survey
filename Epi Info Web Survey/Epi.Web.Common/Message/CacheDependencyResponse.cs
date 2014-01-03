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

        [DataMember]
        public List<CacheDependencyDTO> CacheDependencyList;

        public CacheDependencyResponse() 
        {
            CacheDependencyList = new List<CacheDependencyDTO>();
            SurveyDependency = new Dictionary<string, System.DateTime>();
        }

        public CacheDependencyResponse(List<string> cachedSurveyKeys) : base() 
        {
            CacheDependencyList = new List<CacheDependencyDTO>();
            SurveyDependency = new Dictionary<string, System.DateTime>(); 
        }
    }
}
