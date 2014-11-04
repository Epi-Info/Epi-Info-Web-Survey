using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Epi.Web.Enter.Common.BusinessRule
{
    /// <summary>
    /// Enum of operators that can be used in validation rules.
    /// </summary>
    public enum ValidationOperator
    {
        Equal, 
        NotEqual, 
        GreaterThan, 
        GreaterThanEqual, 
        LessThan, 
        LessThanEqual
    }
}
