using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Epi.Web.Enter.Common.Criteria
{
    /// <summary>
    /// Holds criteria for Organization queries.
    /// </summary>
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class OrganizationCriteria : Criteria
    {
        public OrganizationCriteria()
        {

        }

        [DataMember]
        public Guid AdminstrationKey { get; set; }

        [DataMember]
        public List<string> Organization  { get; set; }

        [DataMember]
        public List<Guid> OrganizationKey{ get; set; }

    }
}
