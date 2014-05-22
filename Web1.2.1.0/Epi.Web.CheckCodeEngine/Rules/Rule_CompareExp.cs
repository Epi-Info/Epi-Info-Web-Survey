using System;
using System.Collections.Generic;
using System.Text;
using com.calitha.goldparser;

namespace Epi.Core.EnterInterpreter.Rules
{
    public class Rule_CompareExp : EnterRule
    {
        EnterRule ConcatExp = null;
        string op = null;
        EnterRule CompareExp = null;
        string STRING = null;
        
        public Rule_CompareExp(Rule_Context pContext, NonterminalToken pToken) : base(pContext)
        {
            // <Concat Exp> LIKE String
            // <Concat Exp> '=' <Compare Exp>
            // <Concat Exp> '<>' <Compare Exp>
            // <Concat Exp> '>' <Compare Exp>
            // <Concat Exp> '>=' <Compare Exp>
            // <Concat Exp> '<' <Compare Exp>
            // <Concat Exp> '<=' <Compare Exp>
            // <Concat Exp>
            
            this.ConcatExp = EnterRule.BuildStatments(pContext, pToken.Tokens[0]);
            if (pToken.Tokens.Length > 1)
            {
                op = pToken.Tokens[1].ToString().ToLower();

                if (pToken.Tokens[1].ToString() == "LIKE")
                {
                    this.STRING = pToken.Tokens[2].ToString();
                    this.CompareExp = EnterRule.BuildStatments(pContext, pToken.Tokens[2]);
                }
                else
                {
                    this.CompareExp = EnterRule.BuildStatments(pContext, pToken.Tokens[2]);
                }
            }
        }


        /// <summary>
        /// perfoms comparison operations on expression ie (=, <=, >=, Like, >, <, and <)) returns a boolean
        /// </summary>
        /// <returns>object</returns>
        public override object Execute()
        {
            object result = null;

            if (op == null)
            {
                result = this.ConcatExp.Execute();
            }
            else
            {
                object LHSO = this.ConcatExp.Execute();
                object RHSO = this.CompareExp.Execute();
                double TryValue = 0.0;
                int i;

                if (Util.IsEmpty(LHSO) && Util.IsEmpty(RHSO) && op.Equals("="))
                {
                    result = true;
                }
                else if (Util.IsEmpty(LHSO) && Util.IsEmpty(RHSO) && op.Equals("<>"))
                {
                    return false;
                }
                else if ((Util.IsEmpty(LHSO) || Util.IsEmpty(RHSO)))
                {
                    if (op.Equals("<>"))
                    {
                        return !(Util.IsEmpty(LHSO) && Util.IsEmpty(RHSO));
                    }
                    else
                    {
                        result = false;
                    }
                }
                else if (op.Equals("LIKE", StringComparison.OrdinalIgnoreCase))
                {
                    //string testValue = "^" + RHSO.ToString().Replace("*", "(\\s|\\w)*") + "$";
                    string testValue = "^" + RHSO.ToString().Replace("*", ".*") + "$";
                    System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex(testValue, System.Text.RegularExpressions.RegexOptions.IgnoreCase);


                    if (re.IsMatch(LHSO.ToString()))
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {

                    if (this.NumericTypeList.Contains(LHSO.GetType().Name.ToUpper()) && this.NumericTypeList.Contains(RHSO.GetType().Name.ToUpper()))
                    {
                        LHSO = Convert.ToDouble(LHSO);
                        RHSO = Convert.ToDouble(RHSO);
                    }

                    if (!LHSO.GetType().Equals(RHSO.GetType()))
                    {
                        if (RHSO is Boolean && op.Equals("="))
                        {
                            result = (RHSO.Equals(!Util.IsEmpty(LHSO)));
                        }
                        else if (LHSO is string && this.NumericTypeList.Contains(RHSO.GetType().Name.ToUpper()) && double.TryParse(LHSO.ToString(), out TryValue))
                        {
                            i = TryValue.CompareTo(RHSO);

                            switch (op)
                            {
                                case "=":
                                    result = (i == 0);
                                    break;
                                case "<>":
                                    result = (i != 0);
                                    break;
                                case "<":
                                    result = (i < 0);
                                    break;
                                case ">":
                                    result = (i > 0);
                                    break;
                                case ">=":
                                    result = (i >= 0);
                                    break;
                                case "<=":
                                    result = (i <= 0);
                                    break;
                            }

                        }
                        else if (RHSO is string && this.NumericTypeList.Contains(LHSO.GetType().Name.ToUpper()) && double.TryParse(RHSO.ToString(), out TryValue))
                        {
                            i = TryValue.CompareTo(LHSO);

                            switch (op)
                            {
                                case "=":
                                    result = (i == 0);
                                    break;
                                case "<>":
                                    result = (i != 0);
                                    break;
                                case "<":
                                    result = (i < 0);
                                    break;
                                case ">":
                                    result = (i > 0);
                                    break;
                                case ">=":
                                    result = (i >= 0);
                                    break;
                                case "<=":
                                    result = (i <= 0);
                                    break;
                            }
                        }
                        else if (op.Equals("=") && (LHSO is Boolean || RHSO is Boolean))
                        {
                            if (LHSO is Boolean && RHSO is Boolean)
                            {
                                result = LHSO == RHSO;   
                            }
                            else if (LHSO is Boolean)
                            {
                                result = (Boolean)LHSO == (Boolean) this.ConvertStringToBoolean(RHSO.ToString());
                            }
                            else
                            {
                                result = (Boolean)this.ConvertStringToBoolean(LHSO.ToString()) == (Boolean)RHSO;
                            }
                        }
                        else
                        {
                            i = StringComparer.CurrentCultureIgnoreCase.Compare(LHSO.ToString(), RHSO.ToString());

                            switch (op)
                            {
                                case "=":
                                    result = (i == 0);
                                    break;
                                case "<>":
                                    result = (i != 0);
                                    break;
                                case "<":
                                    result = (i < 0);
                                    break;
                                case ">":
                                    result = (i > 0);
                                    break;
                                case ">=":
                                    result = (i >= 0);
                                    break;
                                case "<=":
                                    result = (i <= 0);
                                    break;
                            }
                        }
                    }
                    else
                    {
                        i = 0;

                        if (LHSO.GetType().Name.ToUpper() == "STRING" && RHSO.GetType().Name.ToUpper() == "STRING")
                        {
                            i = StringComparer.CurrentCultureIgnoreCase.Compare(LHSO.ToString().Trim(), RHSO.ToString().Trim());
                        }
                        else if (LHSO is IComparable && RHSO is IComparable)
                        {
                            i = ((IComparable)LHSO).CompareTo((IComparable)RHSO);
                        }

                        switch (op)
                        {
                            case "=":
                                result = (i == 0);
                                break;
                            case "<>":
                                result = (i != 0);
                                break;
                            case "<":
                                result = (i < 0);
                                break;
                            case ">":
                                result = (i > 0);
                                break;
                            case ">=":
                                result = (i >= 0);
                                break;
                            case "<=":
                                result = (i <= 0);
                                break;
                        }
                    }
                }
            }
            return result;
        }





        public override void ToJavaScript(StringBuilder pJavaScriptBuilder)
        {
            if (op == null)
            {
                this.ConcatExp.ToJavaScript(pJavaScriptBuilder);
            }
            else
            {
                if (this.op == "like")
                {
                    pJavaScriptBuilder.Append("CCE_Like(");
                    this.ConcatExp.ToJavaScript(pJavaScriptBuilder);
                    pJavaScriptBuilder.Append(",");
                  
                    this.CompareExp.ToJavaScript(pJavaScriptBuilder);
                    pJavaScriptBuilder.Append(")");
                }
                else
                {
                    if (this.ConcatExp is Rule_Value)
                    {
                        WriteValueJavascript((Rule_Value)this.ConcatExp, pJavaScriptBuilder);
                    }
                    else
                    {
                        this.ConcatExp.ToJavaScript(pJavaScriptBuilder);
                    }

                    switch (op)
                    {
                        case "=":
                            pJavaScriptBuilder.Append("==");
                            break;
                        case "<>":
                            pJavaScriptBuilder.Append("!=");
                            break;
                        default:
                            pJavaScriptBuilder.Append(this.op);
                            break;
                    }

                    if (this.CompareExp is Rule_Value)
                    {
                        WriteValueJavascript((Rule_Value)this.CompareExp, pJavaScriptBuilder);
                    }
                    else
                    {
                        this.CompareExp.ToJavaScript(pJavaScriptBuilder);
                    }

                }
            }

        }


        private void WriteValueJavascript(Rule_Value pValue, StringBuilder pJavaScriptBuilder)
        {
            if (!string.IsNullOrEmpty(pValue.Id))
            {
                PluginVariable var = (PluginVariable)this.Context.CurrentScope.resolve(pValue.Id);
                pValue.ToJavaScript(pJavaScriptBuilder);
                if (var != null)
                {
                    switch (var.DataType)
                    {

                        case EpiInfo.Plugin.DataType.Boolean:
                        case EpiInfo.Plugin.DataType.Date:
                        case EpiInfo.Plugin.DataType.DateTime:
                        case EpiInfo.Plugin.DataType.Number:
                        case EpiInfo.Plugin.DataType.Time:
                            break;
                        case EpiInfo.Plugin.DataType.Text:
                        case EpiInfo.Plugin.DataType.GUID:
                        default:
                            pJavaScriptBuilder.Append(".toLowerCase()");
                            break;
                    }
                }
            }
            else
            {
                if (pValue.VariableDataType != EpiInfo.Plugin.DataType.Unknown)
                {
                    pValue.ToJavaScript(pJavaScriptBuilder);
                    switch (pValue.VariableDataType)
                    {
                        case EpiInfo.Plugin.DataType.Boolean:
                        case EpiInfo.Plugin.DataType.Date:
                        case EpiInfo.Plugin.DataType.DateTime:
                        case EpiInfo.Plugin.DataType.Number:
                        case EpiInfo.Plugin.DataType.Time:
                            break;
                        case EpiInfo.Plugin.DataType.Text:
                        default:
                            pJavaScriptBuilder.Append(".toLowerCase()");
                            break;
                    }
                }
                else if (pValue.value is string)
                {
                    pValue.ToJavaScript(pJavaScriptBuilder);
                    pJavaScriptBuilder.Append(".toLowerCase()");
                }
                else
                {
                    pValue.ToJavaScript(pJavaScriptBuilder);
                }
            }
        }

    }
}
