using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
namespace Epi.Web.Common.BusinessObject
    {
    public class StateBO
        {
        private int _StateId;
        private string _StateName;
        private string _StateCode;
          [DataMember]
        public string StateName
            {
            get { return _StateName; }
            set { _StateName = value; }
            }
          [DataMember]
        public string StateCode
            {
            get { return _StateCode; }
            set { _StateCode = value; }
            }
          [DataMember]
        public int StateId
            {
            get { return _StateId; }
            set { _StateId = value; }
            }
        }
    }
