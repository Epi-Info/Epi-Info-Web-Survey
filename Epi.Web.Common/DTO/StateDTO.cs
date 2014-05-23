using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Epi.Web.Common.DTO
    {
    public class StateDTO
        {
        private int _StateId;
        private string _StateName;
        private string _StateCode;

        public string StateName
            {
            get { return _StateName; }
            set { _StateName = value; }
            }
        public string StateCode
            {
            get { return _StateCode; }
            set { _StateCode = value; }
            }
        public int StateId
            {
            get { return _StateId; }
            set { _StateId = value; }
            }
        }
    }
