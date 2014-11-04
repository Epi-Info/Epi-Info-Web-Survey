using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Epi.Web.Enter.Common.BusinessObject
    {
     [DataContract(Namespace = "http://www.yourcompany.com/types/")]
      public  class ResponseXmlBO
        {
        private string _ResponseId;
        private string _Xml;
        private int _User;
        private bool _IsNewRecord;
        [DataMember]
        public string ResponseId
            {
            get { return _ResponseId; }
            set { _ResponseId = value; }
            }

        [DataMember]
        public string Xml
            {
            get { return _Xml; }
            set { _Xml = value; }
            }

        [DataMember]
        public int User
            {
            get { return _User; }
            set { _User = value; }
            }
        
        [DataMember]
        public bool IsNewRecord
            {
            get { return _IsNewRecord; }
            set { _IsNewRecord = value; }
            }
        }
    }
