using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Epi.Web.Common.Constants
{
    public  static class Constant
    {
        public const string WRAPPER = "div";
        public const string FIELDWRAPPERCLASS = "mvcfieldwrapper";
        public const string FORMWRAPPERCLASS = "MvcDynamicForm";
        public const string FIELDPREFIX = "mvcdynamicfield_";
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
