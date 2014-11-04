using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Epi.Web.Enter.Common.BusinessObject
    {
      [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class FormSettingBO
        {
       
        private Dictionary<int, string> _ColumnNameList;
        private Dictionary<int, string> _FormControlNameList;
        private Dictionary<int, string> _AssignedUserList;
        private Dictionary<int, string> _UserList;

       
        [DataMember]
        public Dictionary<int, string> ColumnNameList
            {
            get { return _ColumnNameList; }
            set { _ColumnNameList = value; }
            }
        public Dictionary<int, string> FormControlNameList
            {
            get { return _FormControlNameList; }
            set { _FormControlNameList = value; }
            }

        public Dictionary<int, string> AssignedUserList
            {
            get { return _AssignedUserList; }
            set { _AssignedUserList = value; }
            }
        public Dictionary<int, string> UserList
            {
            get { return _UserList; }
            set { _UserList = value; }
            }


        }
    }
