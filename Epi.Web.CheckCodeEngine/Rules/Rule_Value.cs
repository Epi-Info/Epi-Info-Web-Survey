using System;
using System.Collections.Generic;
using System.Text;
using com.calitha.goldparser;

namespace Epi.Core.EnterInterpreter.Rules
{
    public class Rule_Value : EnterRule
    {
        public string Id = null;
        string Namespace = null;
        public object value = null;
        public EpiInfo.Plugin.DataType VariableDataType;
        bool UseParenthesis = false;

        //object ReturnResult = null;

        public Rule_Value(Rule_Context pContext, Token pToken) : base(pContext)
        {
            /* ::= Identifier	| <Literal> | Boolean | '(' <Expr List> ')' */

            if (pToken is NonterminalToken)
            {
                NonterminalToken T = (NonterminalToken)pToken;
                if (T.Tokens.Length == 1)
                {
                    switch (T.Symbol.ToString())
                    {

                        case "<Qualified ID>":
                        case "Identifier":
                            this.Id = this.GetCommandElement(T.Tokens, 0);
                            break;
                        case "<FunctionCall>":
                            //this.value = new Rule_FunctionCall(pContext, (NonterminalToken)T.Tokens[0]);
                            this.value = EnterRule.BuildStatments(pContext, T.Tokens[0]);
                            break;

                        case "<Literal_Date>":
                            this.VariableDataType = EpiInfo.Plugin.DataType.Date;
                            this.value = this.GetCommandElement(T.Tokens, 0);
                            break;
                        case "<Literal_Time>":
                            this.VariableDataType = EpiInfo.Plugin.DataType.Time;
                            this.value = this.GetCommandElement(T.Tokens, 0);
                            break;
                        case "<Literal>":
                        case "<Literal_String>":
                            this.VariableDataType = EpiInfo.Plugin.DataType.Text;
                            this.value = this.GetCommandElement(T.Tokens, 0);
                            break;
                        case "<Number>":
                        case "<Real_Number>":
                        case "<Decimal_Number>":
                        case "<Hex_Number>":
                            this.VariableDataType = EpiInfo.Plugin.DataType.Number;
                            this.value = this.GetCommandElement(T.Tokens, 0);
                            break;
                        case "<Subroutine_Statement>":

                            break;
                        default:
                            this.value = this.GetCommandElement(T.Tokens, 0);
                            switch (this.value.ToString())
                            {
                                case "(+)":
                                    this.VariableDataType = EpiInfo.Plugin.DataType.Boolean;
                                    this.value = true;
                                    break;
                                case "(-)":
                                    this.VariableDataType = EpiInfo.Plugin.DataType.Boolean;
                                    this.value = false;
                                    break;
                                case "(.)":
                                    this.VariableDataType = EpiInfo.Plugin.DataType.Boolean;
                                    this.value = null;
                                    break;

                            }
                            break;
                    }
                }
                else
                {
                    //this.value = new Rule_ExprList(pContext, (NonterminalToken)T.Tokens[1]);
                    if (T.Tokens.Length == 0)
                    {
                        this.value = EnterRule.BuildStatments(pContext, T);

                    }
                    else if (T.Symbol.ToString() == "<Fully_Qualified_Id>" || T.Symbol.ToString() == "<Qualified ID>")
                    {
                        string[] temp = this.ExtractTokens(T.Tokens).Split(' ');
                        this.Namespace = temp[0];
                        this.Id = temp[2];
                    }
                    else
                    {
                        if (this.GetCommandElement(T.Tokens, 0) == "(")
                        {
                            UseParenthesis = true;
                        }
                        
                        //this.value = new Rule_ExprList(pContext, (NonterminalToken)T.Tokens[1]);
                        this.value = EnterRule.BuildStatments(pContext, T.Tokens[1]);
                        

                    }
                }
            }
            else
            {
                TerminalToken TT = (TerminalToken)pToken;
                switch (TT.Symbol.ToString())
                {
                    case "Boolean":
                        this.VariableDataType = EpiInfo.Plugin.DataType.Boolean;
                        this.value = TT.Text;
                        break;
                    case "RealLiteral":
                    case "DecLiteral":
                    case "HexLiteral":
                        this.VariableDataType = EpiInfo.Plugin.DataType.Number;
                        this.value = TT.Text;
                        break;
                    case "Date":
                        this.VariableDataType = EpiInfo.Plugin.DataType.Date;
                        this.value = TT.Text;
                        break;
                    case "Time":
                        this.VariableDataType = EpiInfo.Plugin.DataType.Time;
                        this.value = TT.Text;
                        break;
                    case "Identifier":
                        this.Id = TT.Text;
                        this.VariableDataType = EpiInfo.Plugin.DataType.Time;
                        break;
                    case "String":
                        this.VariableDataType = EpiInfo.Plugin.DataType.Text;
                        this.value = TT.Text;
                        break;
                    default:
                        this.VariableDataType = EpiInfo.Plugin.DataType.Unknown;
                        this.value = TT.Text;
                        break;
                }
            }

            if (this.Id == null && this.value == null)
            {

            }
        }

        public Rule_Value(string pValue)
        {
            this.value = pValue;
        }


        /// <summary>
        /// performs execution of retrieving the value of a variable or expression
        /// </summary>
        /// <returns>object</returns>
        public override object Execute()
        {
            object result = null;

            if (this.Id != null)
            {
                EpiInfo.Plugin.IVariable var;
                EpiInfo.Plugin.DataType dataType = EpiInfo.Plugin.DataType.Unknown;
                string dataValue = string.Empty;

                var = this.Context.CurrentScope.resolve(this.Id, this.Namespace);

                if (var != null)
                {
                    dataType = var.DataType;
                    dataValue = var.Expression;
                }
                else
                {
                    if (this.Context.EnterCheckCodeInterface != null)
                    {
                        EpiInfo.Plugin.DataType dt = EpiInfo.Plugin.DataType.Unknown;

                        this.Context.EnterCheckCodeInterface.TryGetFieldInfo(this.Id, out dt, out dataValue);
                        dataType = (EpiInfo.Plugin.DataType)dt;
                    }
                }
                result = ConvertEpiDataTypeToSystemObject(dataType, dataValue);
            }
            else
            {
                if (value is EnterRule)
                {
                    result = ((EnterRule)value).Execute();
                }
                else 
                {
                    
                    //result = ParseDataStrings(((String)value));
                    result = ParseDataStrings(value);
                }
            }
            return result;
        }


        private object ConvertEpiDataTypeToSystemObject(EpiInfo.Plugin.DataType dataType, string dataValue)
        {
            object result = null;
            if (dataValue != null)
            {
                switch (dataType)
                {
                    case EpiInfo.Plugin.DataType.Boolean:
                    case EpiInfo.Plugin.DataType.YesNo:
                        result = new Boolean();
                        if (dataValue == "(+)" || dataValue.ToLower() == "true" || dataValue == "1" || dataValue.ToLower() == "yes")
                            result = true;
                        else if (dataValue == "(-)" || dataValue.ToLower() == "false" || dataValue == "0" || dataValue.ToLower() == "no")
                            result = false;
                        else
                            result = null;
                        break;

                    case EpiInfo.Plugin.DataType.Number:
                        double num;
                        if (double.TryParse(dataValue, out num))
                            result = num;
                        else
                            result = null;
                        break;

                    case EpiInfo.Plugin.DataType.Date:
                    case EpiInfo.Plugin.DataType.DateTime:
                    case EpiInfo.Plugin.DataType.Time:
                        DateTime dateTime;
                        if (DateTime.TryParse(dataValue, out dateTime))
                            result = dateTime;
                        else
                            result = null;
                        break;
                    case EpiInfo.Plugin.DataType.PhoneNumber:
                    case EpiInfo.Plugin.DataType.GUID:
                    case EpiInfo.Plugin.DataType.Text:
                        if (dataValue != null)
                            result = dataValue.Trim().Trim('\"');
                        else
                            result = null;
                        break;
                    case EpiInfo.Plugin.DataType.Unknown:
                    default:
                        double double_compare;
                        DateTime DateTime_compare;
                        bool bool_compare;

                        if (double.TryParse(dataValue, out double_compare))
                        {
                            result = double_compare;
                        }
                        else
                            if (DateTime.TryParse(dataValue, out DateTime_compare))
                            {
                                result = DateTime_compare;
                            }
                            else
                                if (bool.TryParse(dataValue, out bool_compare))
                                {
                                    result = bool_compare;
                                }
                                else { result = dataValue; }
                        break;
                }
            }
            return result;
        }

        private object ParseDataStrings(object subject)
        {
            object result = null;
            if (subject is Rule_ExprList)
            {
                result = ((Rule_ExprList)subject).Execute();
            }
            else if (subject is Rule_FunctionCall)
            {
                result = ((Rule_FunctionCall)subject).Execute();
            }
            else if (subject is String)
            {
                Double number;
                DateTime dateTime;

                result = ((String)subject).Trim('\"');
                //removing the "1" and "0" conditions here because an expression like 1 + 0 was evaluating as two booleans
                //if ((String)subject == "1" || (String)subject == "(+)" || ((String)subject).ToLower() == "true")
                if ((String)subject == "(+)" || ((String)subject).ToLower() == "true" || ((String)subject).ToLower() == "yes")
                {
                    result = new Boolean();
                    result = true;
                }
                //else if ((String)subject == "0" || (String)subject == "(-)" || ((String)subject).ToLower() == "false")
                else if ((String)subject == "(-)" || ((String)subject).ToLower() == "false" || ((String)subject).ToLower() == "no")
                {
                    result = new Boolean();
                    result = false;
                }
                else if ((String)subject == "(.)")
                {
                    result = null;
                }
                else if (Double.TryParse(result.ToString(), out number))
                {
                    result = number;
                }
                else if (DateTime.TryParse(result.ToString(), out dateTime))
                {
                    result = dateTime;
                }
            }
            else if (subject is Boolean)
            {
                result = subject;
            }
            return result;
        }


        public override void ToJavaScript(StringBuilder pJavaScriptBuilder)
        {

            if (this.Id != null)
            {
                pJavaScriptBuilder.Append("cce_Context.getValue(\"");
                pJavaScriptBuilder.Append(this.Id.ToLower());
                pJavaScriptBuilder.Append("\")");
            }

            else
            {
                //pJavaScriptBuilder.Append(this.value.ToString());

                if (this.VariableDataType != EpiInfo.Plugin.DataType.Unknown)
                {
                    switch (this.VariableDataType)
                    {
                        case EpiInfo.Plugin.DataType.Boolean:
                            if(this.value == null)
                            {
                                pJavaScriptBuilder.Append("null");
                            }
                            else
                            {
                                Object result = this.ConvertStringToBoolean(this.value.ToString());
                                if(result == null)
                                {
                                    pJavaScriptBuilder.Append("null");
                                }
                                else
                                if ((bool)result)
                                {
                                    pJavaScriptBuilder.Append("true");
                                }
                                else
                                {
                                    pJavaScriptBuilder.Append("false");
                                }
                            }
                            break;
                        case EpiInfo.Plugin.DataType.Date:
                        case EpiInfo.Plugin.DataType.DateTime:
                            pJavaScriptBuilder.Append(string.Format("new Date(\"{0}\").valueOf()",this.value.ToString()));
                            break;
                        case EpiInfo.Plugin.DataType.Number:
                            pJavaScriptBuilder.Append(string.Format("new Number({0}).valueOf()", this.value.ToString()));
                            break;
                        case EpiInfo.Plugin.DataType.Time:
                            pJavaScriptBuilder.Append(string.Format("new Date(\"01/01/1970 {0}\").valueOf()", this.value.ToString()));
                            break;
                        case EpiInfo.Plugin.DataType.Object:
                            if (this.value is EnterRule)
                            {
                                if (this.UseParenthesis)
                                {
                                    pJavaScriptBuilder.Append("("); 
                                    ((EnterRule)this.value).ToJavaScript(pJavaScriptBuilder);
                                    pJavaScriptBuilder.Append(")");
                                }
                                else
                                {

                                    ((EnterRule)this.value).ToJavaScript(pJavaScriptBuilder);
                                }
                            }
                            else
                            {
                                pJavaScriptBuilder.Append(this.value.ToString());
                            }
                            break;
                        case EpiInfo.Plugin.DataType.Text:
                        default:
                            pJavaScriptBuilder.Append(this.value.ToString());
                            break;
                    }
                }
                else 
                {
                    pJavaScriptBuilder.Append(this.value.ToString());
                }

            }
        }

    }
}
