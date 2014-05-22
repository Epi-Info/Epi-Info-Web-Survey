using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.calitha.goldparser;

namespace Epi.Core.EnterInterpreter.Rules
{
    /// <summary>
    /// Reduction to get the system time.
    /// </summary>
    public partial class Rule_SystemTime : EnterRule
    {
        public Rule_SystemTime(Rule_Context pContext, NonterminalToken pToken)
            : base(pContext)
        {
            //SYSTEMTIME
        }

        /// <summary>
        /// Executes the reduction.
        /// </summary>
        /// <returns>Returns the system time.</returns>
        public override object Execute()
        {
            DateTime temp = DateTime.Now;
            TimeSpan result = new TimeSpan(temp.Hour, temp.Minute, temp.Second);
            return result;
        }

        public override void ToJavaScript(StringBuilder pJavaScriptBuilder)
        {
            pJavaScriptBuilder.Append("CCE_SystemTime()");
        }
    }
}
