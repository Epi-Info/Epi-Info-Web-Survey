using System.Collections.Generic;
using System.Runtime.Serialization;

using Epi.Web.Common.MessageBase;
using Epi.Web.Common.DTO;

namespace Epi.Web.Common.Message
{
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class SourceTablesRequest : RequestBase
    {
        public SourceTablesRequest() { }
       
        public string SurveyId { get; set; }

        public List<SourceTableDTO> List { get; set; }
    }
}
