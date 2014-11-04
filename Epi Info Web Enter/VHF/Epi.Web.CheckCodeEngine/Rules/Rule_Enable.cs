using System;
using System.Collections.Generic;
using System.Text;
using com.calitha.goldparser;

namespace Epi.Core.EnterInterpreter.Rules
{
    public class Rule_Enable :EnterRule
    {
        bool IsExceptList = false;
        string[] IdentifierList = null;

        public Rule_Enable(Rule_Context pContext, NonterminalToken pToken)
            : base(pContext)
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
            //this.Context.EnterCheckCodeInterface.Enable(this.IdentifierList, this.IsExceptList);
            if (!this.IsExceptList)
            {
                foreach (string s in this.IdentifierList)
                {
                    if (!this.Context.DisabledFieldList.Contains(s.ToLower()))
                    {
                        this.Context._DisabledFieldList.Remove(s.ToLower());
                    }
                }
            }
            else
            {
                Dictionary<string, string> FieldChecker = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                foreach (string s in this.IdentifierList)
                {
                    if (!FieldChecker.ContainsKey(s))
                    {
                        FieldChecker.Remove(s.ToLower() );
                    }
                }

                foreach (EpiInfo.Plugin.IVariable v in this.Context.CurrentScope.FindVariables(EpiInfo.Plugin.VariableScope.DataSource))
                {
                    string key = v.Name.ToLower();

                    if (!this.Context._DisabledFieldList.Contains(key) && !FieldChecker.ContainsKey(key))
                    {
                        this.Context._DisabledFieldList.Remove(key);
                    }
                }

            }
            return null;
        }

         public override void ToJavaScript(StringBuilder pJavaScriptBuilder)
        {
            pJavaScriptBuilder.AppendLine("var List = new Array();");
            List<string> FieldList = new List<string>(this.IdentifierList);
            this.Context.ExpandGroupVariables(FieldList, ref this.IsExceptList);
            foreach (string fieldName in FieldList)
            {
                pJavaScriptBuilder.AppendLine(string.Format("List.push('{0}');", fieldName.ToLower()));
            }
            //result.AppendLine("List.push('MvcDynamicField_Ill');");
            if (this.IsExceptList)
            {
                pJavaScriptBuilder.AppendLine("CCE_Enable(List,true);");
            }
            else
            {
                pJavaScriptBuilder.AppendLine("CCE_Enable(List,false);");
            }

        }
    }
}
