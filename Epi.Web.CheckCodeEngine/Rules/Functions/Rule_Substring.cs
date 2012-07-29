using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.calitha.goldparser;

namespace Epi.Core.EnterInterpreter.Rules
{
    public partial class Rule_Substring : EnterRule
    {
        
        private List<EnterRule> ParameterList = new List<EnterRule>();


        public Rule_Substring(Rule_Context pContext, NonterminalToken pToken)
            : base(pContext)
        {
            //SUBSTRING(fullString,startingIndex,length)
            this.ParameterList = EnterRule.GetFunctionParameters(pContext, pToken);
            
        }
        /// <summary>
        /// returns a substring index is 1 based ie 1 = first character
        /// </summary>
        /// <returns>object</returns>
        public override object Execute()
        {
            object result = null;
            object fullString = null;
            object startIndex = 0;
            object length = 0;

            fullString = this.ParameterList[0].Execute();
            startIndex = this.ParameterList[1].Execute();
            length = this.ParameterList[2].Execute();

            if (!Util.IsEmpty(fullString))
            {
                if (fullString.ToString().Length >= int.Parse(startIndex.ToString()) - 1 + int.Parse(length.ToString()))
                {
                    result = fullString.ToString().Substring(int.Parse(startIndex.ToString()) - 1, int.Parse(length.ToString()));
                }
            }

            return result;
        }

        public override void ToJavaScript(StringBuilder pJavaScriptBuilder)
        {
            pJavaScriptBuilder.Append("CCE_Substring(");
            this.ParameterList[0].ToJavaScript(pJavaScriptBuilder);
            pJavaScriptBuilder.Append(",");
            this.ParameterList[1].ToJavaScript(pJavaScriptBuilder);
            pJavaScriptBuilder.Append(",");
            this.ParameterList[2].ToJavaScript(pJavaScriptBuilder);
            pJavaScriptBuilder.Append(")");
        }
    }
}
