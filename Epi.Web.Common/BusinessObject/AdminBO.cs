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
        private string _AdressLine1;
        private string _AdressLine2;
        private string _City;
        private int _StateId;
        private string _Zip;
        private int _AddressId;
        private string _UserName;
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
         [DataMember]
          public string AdressLine1
              {
              get { return _AdressLine1; }
              set { _AdressLine1 = value; }
              }
         [DataMember]
          public string AdressLine2
              {
              get { return _AdressLine2; }
              set { _AdressLine2 = value; }
              }
         [DataMember]
          public string City
              {
              get { return _City; }
              set { _City = value; }
              }
         [DataMember]
         public int StateId
              {
              get { return _StateId; }
              set { _StateId = value; }
              }
         [DataMember]
          public string Zip
              {
              get { return _Zip; }
              set { _Zip = value; }
              }
         [DataMember]
         public int AddressId
             {
             get { return _AddressId; }
             set { _AddressId = value; }
             }
        [DataMember]
        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }
    }
    }
