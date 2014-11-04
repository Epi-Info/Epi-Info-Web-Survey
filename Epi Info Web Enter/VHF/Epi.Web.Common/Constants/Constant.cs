using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Epi.Web.Enter.Common.Constants
{
    public  static class Constant
    {
        public enum OperationMode
        {
            NoChange =0,
            UpdatePassword = 1,
            UpdateUserInfo = 2
          
        }

        public enum EmailCombinationEnum
        {
            ResetPassword = 1,
            PasswordChanged = 2,
            UpdateUserInfo = 3,
            InsertUser = 4,
            UpdateOrganization =5,
            InsertOrganization = 6
        }
    }
}
