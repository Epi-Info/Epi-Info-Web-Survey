using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Epi.Web.Common.DTO
{
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class PublishInfoDTO  
    {
        bool isPulished;
        string uRL;
        string statusText;
        Dictionary<int, string> viewIdAndFormIdList;
        [DataMember]
        public bool IsPulished { get { return this.isPulished; } set { this.isPulished = value; } }

        [DataMember]
        public string URL { get { return this.uRL; } set { this.uRL = value; } }

        [DataMember]
        public string StatusText { get { return this.statusText; } set { this.statusText = value; } }
        [DataMember]
        public Dictionary<int, string> ViewIdAndFormIdList { get { return this.viewIdAndFormIdList; } set { this.viewIdAndFormIdList = value; } }
    }
}
