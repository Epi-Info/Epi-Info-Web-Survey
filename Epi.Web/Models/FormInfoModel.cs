using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Epi.Web.MVC.Models
{
    public class FormInfoModel
    {
        private string _FormId;
        private string _FormNumber;
        private string _FormName;
        private int _UserId;
        private string _OrganizationName;
        private int _OrganizationId;
        private bool _IsDraftMode;
        private bool _IsOwner;
        private string _OwnerLName;
        private string _OwnerFName;
        private bool _isSQLProject;
        private bool _IsShareable;
        private bool _IsShared;
        private bool _DataAccessRuleId;
        public bool DataAccessRuleId
        {
            get { return _DataAccessRuleId; }
            set { _DataAccessRuleId = value; }
        }

        private bool _ewavLiteToggleSwitch;

        public bool IsSQLProject
        {
            get { return _isSQLProject; }
            set { _isSQLProject = value; }
        }

        public string FormId
        {
            get { return _FormId; }
            set { _FormId = value; }
        }
        public string FormNumber
        {
            get { return _FormNumber; }
            set { _FormNumber = value; }
        }

        public string FormName
        {
            get { return _FormName; }
            set { _FormName = value; }
        }
        public string OrganizationName
        {
            get { return _OrganizationName; }
            set { _OrganizationName = value; }
        }
        public int OrganizationId
        {
            get { return _OrganizationId; }
            set { _OrganizationId = value; }
        }
        public bool IsDraftMode
        {
            get { return _IsDraftMode; }
            set { _IsDraftMode = value; }
        }
        public bool IsOwner
        {
            get { return _IsOwner; }
            set { _IsOwner = value; }
        }
        public int UserId
            {
            get { return _UserId; }
            set { _UserId = value; }
            }
       public string OwnerLName
           {
           get { return _OwnerLName; }
           set { _OwnerLName = value; }
           }
        
       public string OwnerFName
           {
           get { return _OwnerFName; }
           set { _OwnerFName = value; }
           }
       public bool IsShareable
       {
           get { return _IsShareable; }
           set { _IsShareable = value; }
       }

       public bool IsShared
       {
           get { return _IsShared; }
           set { _IsShared = value; }
       }
        public bool EwavLiteToggleSwitch
        {
            get { return _ewavLiteToggleSwitch; }
            set { _ewavLiteToggleSwitch = value; }
        }
    }
}
