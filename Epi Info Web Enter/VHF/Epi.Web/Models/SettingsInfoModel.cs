using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Epi.Web.MVC.Models
    {
    public class SettingsInfoModel
        {
        private Dictionary<int,string> _FormControlNameList;
        private Dictionary<int, string> _SelectedControlNameList;
        private string _FormOwnerFirstName;
        private string _FormOwnerLastName;
        private bool _IsDraftMode;
        private string _FormName;
        private Dictionary<int, string> _AssignedUserList;
        private Dictionary<int, string> _UserList;
        private string _FormId;
       

        public Dictionary<int, string> FormControlNameList
            {
            get { return _FormControlNameList; }
            set { _FormControlNameList = value; }
            }
        
        public Dictionary<int, string> SelectedControlNameList
            {
            get { return _SelectedControlNameList; }
            set { _SelectedControlNameList = value; }
            }
        
        public string FormOwnerFirstName
            {
            get { return _FormOwnerFirstName; }
            set { _FormOwnerFirstName = value; }
            }
         
        public string FormOwnerLastName
            {
            get { return _FormOwnerLastName; }
            set { _FormOwnerLastName = value; }
            }

        public bool IsDraftMode
            {
            get { return _IsDraftMode; }
            set { _IsDraftMode = value; }
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

        public string FormName
            {
            get { return _FormName; }
            set { _FormName = value; }
            }
        public string FormId
            {
            get { return _FormId; }
            set { _FormId = value; }
            }
        }
    }