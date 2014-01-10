using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Epi.Web.Common.BusinessObject
    {
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class AdminBO
        {
        private string _AdminEmail;
        private int _OrganizationId;
        private bool _IsActive;
        private bool _Notify;
        private Guid _AdminId;
        private string _FirstName;
        private string _LastName;
        private string _PhoneNumber;
        [DataMember]
        public string AdminEmail
            {
            get { return _AdminEmail; }
            set { _AdminEmail = value; }
            }

        [DataMember]
        public int OrganizationId
            {
            get { return _OrganizationId; }
            set { _OrganizationId = value; }
            }

        [DataMember]
        public bool IsActive
            {
            get { return _IsActive; }
            set { _IsActive = value; }
            }
        [DataMember]
        public bool Notify
            {
            get { return _Notify; }
            set { _Notify = value; }
            }
        [DataMember]
        public Guid AdminId
            {
            get { return _AdminId; }
            set { _AdminId = value; }
            }
          [DataMember]
        public string FirstName
            {
            get { return _FirstName; }
            set { _FirstName = value; }
            }
          [DataMember]
        public string LastName
            {
            get { return _LastName; }
            set { _LastName = value; }
            }
          [DataMember]
        public string PhoneNumber
            {
            get { return _PhoneNumber; }
            set { _PhoneNumber = value; }
            }
        }
    }
