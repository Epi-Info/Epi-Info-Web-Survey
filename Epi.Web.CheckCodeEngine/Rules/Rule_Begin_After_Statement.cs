using System;
using System.Collections.Generic;
using System.Text;
using com.calitha.goldparser;
namespace Epi.Core.EnterInterpreter.Rules
{
    public class Rule_Begin_After_Statement :EnterRule
    {
        public EnterRule Statements = null;

        public Rule_Begin_After_Statement(Rule_Context pContext, NonterminalToken pToken)
            : base(pContext)
        {
            //<Begin_After_statement> ::= Begin-After <Statements> End  | Begin-After End | !Null
            if (pToken.Tokens.Length > 2)
            {
                //NonterminalToken T = (NonterminalToken)pToken.Tokens[1];
                //this.Statements = new Rule_Statements(pContext, T);
                this.Statements = EnterRule.BuildStatments(pContext, pToken.Tokens[1]);
            }
        }

        /// <summary>
        /// performs execute command
        /// </summary>
        /// <returns>object</returns>
        public override object Execute()
        {
            object result = null;

            //if (this.Statements != null && this.Context.EnterCheckCodeInterface.IsExecutionEnabled)
            if (this.Statements != null)
            {
                try
                {
                    result = this.Statements.Execute();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return result;
        }


        /// <summary>
        /// To String method
        /// </summary>
        /// <returns>object</returns>
        public override string ToString()
        {
            return base.ToString();

        }

        public override bool IsNull() 
        {
            return this.Statements == null;
        }

        public override void ToJavaScript(StringBuilder pJavaScriptBuilder)
        {
            pJavaScriptBuilder.AppendLine("_after(id)");
            pJavaScriptBuilder.AppendLine("{");

            if (this.Statements != null)
            {
                try
                {
                    this.Statements.ToJavaScript(pJavaScriptBuilder);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }


            pJavaScriptBuilder.AppendLine("}");

        }
    }
}
