using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.calitha.goldparser;

namespace Epi.Core.EnterInterpreter.Rules
{
    /// <summary>
    /// Class for the Rule_Abs reduction.
    /// </summary>
    public partial class Rule_Rnd : EnterRule
    {
        private List<EnterRule> ParameterList = new List<EnterRule>();
        private static Random random = new Random(DateTime.Now.Millisecond);

        public Rule_Rnd(Rule_Context pContext, NonterminalToken pToken)
            : base(pContext)
        {
            this.ParameterList = EnterRule.GetFunctionParameters(pContext, pToken);
        }

        /// <summary>
        /// Executes the reduction.
        /// </summary>
        /// <returns>Returns the absolute value of two numbers.</returns>
        public override object Execute()
        {
            object result = null;
            
            object p1 = this.ParameterList[0].Execute().ToString();
            object p2 = this.ParameterList[1].Execute().ToString();

            int param1;
            int param2;


            if(int.TryParse(p1.ToString(), out param1) && int.TryParse(p2.ToString(), out param2))
            {
                
                result = random.Next(param1, param2);

            }

            return result;
        }


        public override void ToJavaScript(StringBuilder pJavaScriptBuilder)
        {
            //Math.floor((Math.random() * 10) + 1);
            pJavaScriptBuilder.Append("Math.floor((Math.random() * ");
            this.ParameterList[1].ToJavaScript(pJavaScriptBuilder);
            pJavaScriptBuilder.Append(") + ");
            this.ParameterList[0].ToJavaScript(pJavaScriptBuilder);
            pJavaScriptBuilder.Append(")");


        }
    }
}
