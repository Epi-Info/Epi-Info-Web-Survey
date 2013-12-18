using System.Runtime.Serialization;
using System.Collections.Generic;
using Epi.Web.Common.MessageBase;
using Epi.Web.Common.Criteria;
using Epi.Web.Common.DTO;

namespace Epi.Web.Common.Message
{
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class CacheDependencyRequest : RequestBase
    {
        public CacheDependencyRequest()
        {
            this.Criteria = new CacheDependencyCriteria();
            this.SurveyDependencyList = new List<CacheDependencyDTO>();
        }

        [DataMember]
        public CacheDependencyCriteria Criteria;

        [DataMember]
        public List<CacheDependencyDTO> SurveyDependencyList;
    }
}
