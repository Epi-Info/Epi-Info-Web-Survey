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
                op = pToken.Tokens[1].ToString();

                if (pToken.Tokens[1].ToString() == "LIKE")
                {
                    this.STRING = pToken.Tokens[2].ToString();
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
                else if (Util.IsEmpty(LHSO) || Util.IsEmpty(RHSO))
                {
                    result = false;
                }
                else if (op.Equals("LIKE", StringComparison.OrdinalIgnoreCase) && LHSO is String && RHSO is String)
                {
                    string testValue = "^" + ((String)RHSO).Replace("*", "\\w*") + "$";
                    System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex(testValue, System.Text.RegularExpressions.RegexOptions.IgnoreCase);


                    if (re.IsMatch(((String)LHSO)))
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
                            i = StringComparer.CurrentCultureIgnoreCase.Compare(LHSO, RHSO);
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

                this.ConcatExp.ToJavaScript(pJavaScriptBuilder);
                if (this.op == "=")
                {
                    pJavaScriptBuilder.Append("==");
                }
                else
                {
                    pJavaScriptBuilder.Append(this.op);
                }
                this.CompareExp.ToJavaScript(pJavaScriptBuilder);
            }

        }

    }
}
