using System;
using System.Collections.Generic;
using System.Text;
using com.calitha.goldparser;

namespace Epi.Core.EnterInterpreter.Rules
{
    public class Rule_Clear :EnterRule
    {
        string[] IdentifierList = null;

        public Rule_Clear(Rule_Context pContext, NonterminalToken pToken) : base(pContext)
        {
            //<Clear_Statement>	::= CLEAR <IdentifierList>
            this.IdentifierList = this.GetCommandElement(pToken.Tokens, 1).ToString().Split(' ');
        }

        /// <summary>
        /// uses the EnterCheckCodeInterface to perform a clear command
        /// </summary>
        /// <returns>object</returns>
        public override object Execute()
        {
            foreach (string s in this.IdentifierList)
            {
                this.Context.CurrentScope.resolve(s).Expression = "";
            }

            return null;
        }

         
        public override void ToJavaScript(StringBuilder pJavaScriptBuilder)
        {
            List<string> FieldList = new List<string>(this.IdentifierList);
            bool IsExceptList = false;
            this.Context.ExpandGroupVariables(FieldList, ref IsExceptList);
            pJavaScriptBuilder.AppendLine("var List = new Array();");

            foreach (string fieldName in FieldList)
            {
                pJavaScriptBuilder.AppendLine(string.Format("List.push('{0}');", fieldName.ToLower()));
            }
            
            pJavaScriptBuilder.AppendLine("CCE_ClearControlValue(List,false);");

        }

    }
}
