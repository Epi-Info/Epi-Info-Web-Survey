using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Epi.Web.Enter.Common.DTO
{
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class FormInfoDTO
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
        [DataMember]
        public string FormId
        {
            get { return _FormId; }
            set { _FormId = value; }
        }
        [DataMember]
        public string FormNumber
        {
            get { return _FormNumber; }
            set { _FormNumber = value; }
        }

        [DataMember]
        public string FormName
        {
            get { return _FormName; }
            set { _FormName = value; }
        }
        [DataMember]
        public string OrganizationName
        {
            get { return _OrganizationName; }
            set { _OrganizationName = value; }
        }
        [DataMember]
        public int OrganizationId
        {
            get { return _OrganizationId; }
            set { _OrganizationId = value; }
        }
        [DataMember]
        public bool IsDraftMode
        {
            get { return _IsDraftMode; }
            set { _IsDraftMode = value; }
        }

        [DataMember]
        public int UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }
        [DataMember]
        public bool IsOwner
        {
            get { return _IsOwner; }
            set { _IsOwner = value; }
        }
        [DataMember]
        public string OwnerLName
        {
            get { return _OwnerLName; }
            set { _OwnerLName = value; }
        }
        [DataMember]
        public string OwnerFName
        {
            get { return _OwnerFName; }
            set { _OwnerFName = value; }
        }

        [DataMember]
        public bool IsSQLProject
        {
            get { return _isSQLProject; }
            set { _isSQLProject = value; }
        }
    }
}
