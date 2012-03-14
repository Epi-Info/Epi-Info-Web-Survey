using System;
using System.Collections.Generic;
using System.Text;
using com.calitha.goldparser;

namespace Epi.Core.EnterInterpreter.Rules
{
    public class Rule_Hide :EnterRule
    {
        bool IsExceptList = false;
        string[] IdentifierList = null;

        public Rule_Hide(Rule_Context pContext, NonterminalToken pToken) : base(pContext)
        {
            //<IdentifierList> ::= <IdentifierList> Identifier | Identifier

            if (pToken.Tokens.Length > 2)
            {
                //<Hide_Except_Statement> ::= HIDE '*' EXCEPT <IdentifierList>
                this.IsExceptList = true;
                this.IdentifierList = this.GetCommandElement(pToken.Tokens, 3).ToString().Split(' ');
            }
            else
            {
                //<Hide_Some_Statement> ::= HIDE <IdentifierList>
                this.IdentifierList = this.GetCommandElement(pToken.Tokens, 1).ToString().Split(' ');
            }
        }


        /// <summary>
        /// performs execution of the HIDE command via the EnterCheckCodeInterface.Hide method
        /// </summary>
        /// <returns>object</returns>
        public override object Execute()
        {
            //this.Context.EnterCheckCodeInterface.Hide(this.IdentifierList, this.IsExceptList);

            if (!this.IsExceptList)
            {
                foreach (string s in this.IdentifierList)
                {
                    if (!this.Context.HiddenFieldList.Contains(s.ToLower()))
                    {
                        this.Context._HiddenFieldList.Add(s.ToLower());
                    }
                }
            }

            return null;
        }


        public override void ToJavaScript(StringBuilder pJavaScriptBuilder)
        {
            pJavaScriptBuilder.AppendLine("var List = new Array();");
            foreach (string fieldName in IdentifierList)
            {
                pJavaScriptBuilder.AppendLine(string.Format("List.push('{0}');", fieldName.ToLower()));
            }
            //result.AppendLine("List.push('MvcDynamicField_Ill');");
            if (this.IsExceptList)
            {
                pJavaScriptBuilder.AppendLine("CCE_Hide(List,true);");
            }
            else
            {
                pJavaScriptBuilder.AppendLine("CCE_Hide(List,false);");
            }

        }

    }
}
