using System;
using System.Collections.Generic;
using System.Text;
using com.calitha.goldparser;

namespace Epi.Core.EnterInterpreter.Rules
{
    public class Rule_UnHide : EnterRule
    {
        bool IsExceptList = false;
        string[] IdentifierList = null;

        public Rule_UnHide(Rule_Context pContext, NonterminalToken pToken) : base(pContext)
        {
            if (pToken.Tokens.Length > 2)
            {
                //<Unhide_Except_Statement> ::= UNHIDE '*' EXCEPT <IdentifierList>
                this.IsExceptList = true;
                this.IdentifierList = this.GetCommandElement(pToken.Tokens, 3).ToString().Split(' ');
            }
            else
            {
                //<Unhide_Some_Statement> ::= UNHIDE <IdentifierList>
                this.IdentifierList = this.GetCommandElement(pToken.Tokens,1).ToString().Split(' ');
            }
        }
        /// <summary>
        /// performs execution of the UNHIDE command via the EnterCheckCodeInterface.UnHide method
        /// </summary>
        /// <returns>object</returns>
        public override object Execute()
        {
            //this.Context.EnterCheckCodeInterface.UnHide(this.IdentifierList, this.IsExceptList);

            if (!this.IsExceptList)
            {
                foreach (string s in this.IdentifierList)
                {
                    if (!this.Context.HiddenFieldList.Contains(s.ToLower()))
                    {
                        this.Context._HiddenFieldList.Remove(s.ToLower());
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
                        FieldChecker.Remove(s.ToLower());
                    }
                }

                foreach (EpiInfo.Plugin.IVariable v in this.Context.CurrentScope.FindVariables(EpiInfo.Plugin.VariableScope.DataSource))
                {
                    string key = v.Name.ToLower();

                    if (!this.Context.HiddenFieldList.Contains(key) && !FieldChecker.ContainsKey(key))
                    {
                        this.Context._HiddenFieldList.Remove(key);
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
                pJavaScriptBuilder.AppendLine("CCE_UnHide(List,true);");
            }
            else
            {
                pJavaScriptBuilder.AppendLine("CCE_UnHide(List,false);");
            }

        }

    }
}
