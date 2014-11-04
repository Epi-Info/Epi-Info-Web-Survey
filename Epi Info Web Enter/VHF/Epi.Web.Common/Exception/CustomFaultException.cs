using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Epi.Web.Enter.Common.Exception
{
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class CustomFaultException
    {
        private string _customMessage;
        private string _source;
        private string _stackTrace;
        private string _helpLink;
        
        [DataMember]
        public string CustomMessage
        {
            get { return _customMessage; }
            set { _customMessage = value; }
        }

        [DataMember]
        public string Source
        {
            get { return _source; }
            set { _source = value; }
        }

        [DataMember]
        public string StackTrace
        {
            get { return _stackTrace; }
            set { _stackTrace = value; }
        }

        [DataMember]
        public string HelpLink
        {
            get { return _helpLink; }
            set { _helpLink = value; }
        }
    }
}
