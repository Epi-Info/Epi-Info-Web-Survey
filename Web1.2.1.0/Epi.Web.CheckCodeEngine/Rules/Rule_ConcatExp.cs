using System;
using System.Collections.Generic;
using System.Text;
using com.calitha.goldparser;

namespace Epi.Core.EnterInterpreter.Rules
{
    public class Rule_ConcatExp: EnterRule
    {
        EnterRule AddExp = null;
        string op = null;
        EnterRule ConcatExp = null;

        public Rule_ConcatExp(Rule_Context pContext, NonterminalToken pToken) : base(pContext)
        {
            /*::= <Add Exp> '&' <Concat Exp>
		   						| <Add Exp>*/

            this.AddExp = EnterRule.BuildStatments(pContext, pToken.Tokens[0]);
            if (pToken.Tokens.Length > 1)
            {
                this.op = pToken.Tokens[1].ToString();

                this.ConcatExp = EnterRule.BuildStatments(pContext, pToken.Tokens[2]);
            }

        }
        /// <summary>
        /// performs concatenation of string via the '&' operator
        /// </summary>
        /// <returns>object</returns>
        public override object Execute()
        {
            object result = null;

            if (op == null)
            {
                result = this.AddExp.Execute();
            }
            else
            {
                object LHSO = this.AddExp.Execute(); 
                object RHSO = this.ConcatExp.Execute();

                if (LHSO != null && RHSO != null)
                {
                    result = LHSO.ToString() + RHSO.ToString();
                }
                else if (LHSO != null)
                {
                    if (LHSO.GetType() == typeof(string))
                    {
                        result = LHSO;
                    }
                }
                else if (RHSO.GetType() == typeof(string))
                {
                    result = RHSO;
                }
            }

            return result;
        }

        public override void ToJavaScript(StringBuilder pJavaScriptBuilder)
        {

            if (op == null)
            {
                if (this.AddExp is Rule_Value)
                {
                    WriteValueJavascript((Rule_Value)this.AddExp, pJavaScriptBuilder);
                }
                else
                {
                    this.AddExp.ToJavaScript(pJavaScriptBuilder);
                }
            }
            else
            {

                /*::= <Add Exp> '&' <Concat Exp>
                    | <Add Exp>*/


                if (this.ConcatExp != null)
                {
                    if (this.AddExp is Rule_Value)
                    {
                        WriteValueJavascript((Rule_Value)this.AddExp, pJavaScriptBuilder);
                    }
                    else
                    {
                        this.AddExp.ToJavaScript(pJavaScriptBuilder);
                    }

                    pJavaScriptBuilder.Append("+");

                    if (this.ConcatExp is Rule_Value)
                    {
                        WriteValueJavascript((Rule_Value)this.ConcatExp, pJavaScriptBuilder);
                    }
                    else
                    {
                        this.ConcatExp.ToJavaScript(pJavaScriptBuilder);
                    }

                }
                else
                {
                    if (this.AddExp is Rule_Value)
                    {
                        WriteValueJavascript((Rule_Value)this.AddExp, pJavaScriptBuilder);
                    }
                    else
                    {
                        this.AddExp.ToJavaScript(pJavaScriptBuilder);
                    }
                }


                

            }

        }


        private void WriteValueJavascript(Rule_Value pValue, StringBuilder pJavaScriptBuilder)
        {
            if (!string.IsNullOrEmpty(pValue.Id))
            {
                PluginVariable var = (PluginVariable)this.Context.CurrentScope.resolve(pValue.Id);
                
                if (var != null)
                {
                    switch (var.DataType)
                    {

                        case EpiInfo.Plugin.DataType.Boolean:
                        case EpiInfo.Plugin.DataType.Date:
                        case EpiInfo.Plugin.DataType.DateTime:
                        case EpiInfo.Plugin.DataType.Number:
                        case EpiInfo.Plugin.DataType.Time:
                        case EpiInfo.Plugin.DataType.Text:
                        case EpiInfo.Plugin.DataType.GUID:
                            pValue.ToJavaScript(pJavaScriptBuilder);
                            break;
                        default:
                            pValue.ToJavaScript(pJavaScriptBuilder);
                            break;
                    }
                }
            }
            else
            {
                if (pValue.VariableDataType != EpiInfo.Plugin.DataType.Unknown)
                {
                    switch (pValue.VariableDataType)
                    {
                        case EpiInfo.Plugin.DataType.Boolean:
                        case EpiInfo.Plugin.DataType.Date:
                        case EpiInfo.Plugin.DataType.DateTime:
                        case EpiInfo.Plugin.DataType.Number:
                        case EpiInfo.Plugin.DataType.Time:
                            pValue.ToJavaScript(pJavaScriptBuilder);
                            break;
                        case EpiInfo.Plugin.DataType.Text:
                        default:
                            pValue.ToJavaScript(pJavaScriptBuilder);
                            break;
                    }
                }
                else if (pValue.value is string)
                {
                    pValue.ToJavaScript(pJavaScriptBuilder);
                }
                else
                {
                    pValue.ToJavaScript(pJavaScriptBuilder);
                }
            }
        }

    }
}
