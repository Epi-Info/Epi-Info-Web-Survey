using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Epi.Web.Enter.Common.BusinessRule
{
    /// <summary>
    /// Represents a validation rules that states that a value is required.
    /// </summary>
    public class ValidateRequired : BusinessRule
    {

        public ValidateRequired(string propertyName)
            : base(propertyName)
        {
            ErrorMessage = propertyName + " is a required field.";
        }

        public ValidateRequired(string propertyName, string errorMessage)
            : base(propertyName)
        {
            ErrorMessage = errorMessage;
        }

        public override bool Validate(BusinessObject.BusinessObject businessObject)
        {
            try
            {
                return GetPropertyValue(businessObject).ToString().Length > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
