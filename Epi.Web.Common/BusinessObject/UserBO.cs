using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Epi.Web.Common.BusinessObject
{
   public class UserBO
    {
        private int _UserId;

        public int UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }

        private string _UserName;

        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }

        private string _FirstName;

        public string FirstName
        {
            get { return _FirstName; }
            set { _FirstName = value; }
        }

        private string _LastName;

        public string LastName
        {
            get { return _LastName; }
            set { _LastName = value; }
        }

        private string _PasswordHash;

        public string PasswordHash
        {
            get { return _PasswordHash; }
            set { _PasswordHash = value; }
        }

        private bool _ResetPassword;

        public bool ResetPassword
        {
            get { return _ResetPassword; }
            set { _ResetPassword = value; }
        }

        private string _EmailAddress;

        public string EmailAddress
        {
            get { return _EmailAddress; }
            set { _EmailAddress = value; }
        }

        private string _PhoneNumber;

        public string PhoneNumber
        {
            get { return _PhoneNumber; }
            set { _PhoneNumber = value; }
        }

        private int _Role;

        public int Role
        {
            get { return _Role; }
            set { _Role = value; }
        }

        private Epi.Web.Common.Constants.Constant.OperationMode updateMode;

        public Epi.Web.Common.Constants.Constant.OperationMode Operation
        {
            get { return updateMode; }
            set { updateMode = value; }
        }

        private bool _IsActive;

        public bool IsActive
            {
            get { return _IsActive; }
            set { _IsActive = value; }
            }
        private int _UserHighestRole;
        public int UserHighestRole
        {
            get { return _UserHighestRole; }
            set { _UserHighestRole = value; }
        }

        private Guid _UGuid;

        public Guid UGuid
        {
            get { return _UGuid; }
            set { _UGuid = value; }
        }


    }
}
