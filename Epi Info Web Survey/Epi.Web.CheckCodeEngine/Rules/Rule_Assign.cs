using System;
using System.Collections.Generic;
using System.Text;
using com.calitha.goldparser;
//using Epi.Data;
using EpiInfo.Plugin;
using System.Globalization;
using System.Threading;

namespace Epi.Core.EnterInterpreter.Rules
{
    public class Rule_Assign : EnterRule
    {
        public string QualifiedId;
        private string Namespace = null;
        //private string functionCall;
        //private string Identifier;
        EnterRule value = null;
        //Epi.View View = null;

        //object ReturnResult = null;

        public Rule_Assign(Rule_Context pContext, NonterminalToken pTokens) : base(pContext)
        {
            //ASSIGN <Qualified ID> '=' <Expression>
            //<Let_Statement> ::= LET Identifier '=' <Expression> 
            //<Simple_Assign_Statement> ::= Identifier '=' <Expression>
            //<Assign_DLL_Statement> ::= ASSIGN <Qualified ID> '=' identifier'!'<FunctionCall>
            string[] temp;
            NonterminalToken T;
            switch(pTokens.Rule.Lhs.ToString())
            {
                  
                case "<Assign_Statement>":
                    T = (NonterminalToken)pTokens.Tokens[1];
                    if (T.Symbol.ToString() == "<Fully_Qualified_Id>")
                    {
                        temp = this.ExtractTokens(T.Tokens).Split(' ');
                        this.Namespace = T.Tokens[0].ToString();
                        this.QualifiedId = this.GetCommandElement(T.Tokens,2);
                    }
                    else
                    {
                        this.QualifiedId = this.GetCommandElement(T.Tokens, 0);
                    }
                    this.value = EnterRule.BuildStatments(pContext, pTokens.Tokens[3]);
                    if (!this.Context.AssignVariableCheck.ContainsKey(this.QualifiedId.ToLower()))
                    {
                        this.Context.AssignVariableCheck.Add(this.QualifiedId.ToLower(), this.QualifiedId.ToLower());
                    }
                    break;
                case "<Let_Statement>":
                    T = (NonterminalToken)pTokens.Tokens[1];
                    if (T.Symbol.ToString() == "<Fully_Qualified_Id>")
                    {
                        temp = this.ExtractTokens(T.Tokens).Split(' ');
                        this.Namespace = T.Tokens[0].ToString();
                        this.QualifiedId = this.GetCommandElement(T.Tokens, 2);
                    }
                    else
                    {
                        this.QualifiedId = this.GetCommandElement(T.Tokens, 0);
                    }

                    
                    this.value = EnterRule.BuildStatments(pContext, pTokens.Tokens[3]);
                    if (!this.Context.AssignVariableCheck.ContainsKey(this.QualifiedId.ToLower()))
                    {
                        this.Context.AssignVariableCheck.Add(this.QualifiedId.ToLower(), this.QualifiedId.ToLower());
                    }
                    break;
                case "<Simple_Assign_Statement>":
                    //Identifier '=' <Expression>
                    //T = (NonterminalToken)pTokens.Tokens[1];
                    T = (NonterminalToken)pTokens.Tokens[0];
                    if (T.Symbol.ToString() == "<Fully_Qualified_Id>")
                    {
                        temp = this.ExtractTokens(T.Tokens).Split(' ');
                        this.Namespace = T.Tokens[0].ToString();
                        this.QualifiedId = this.GetCommandElement(T.Tokens, 2);
                    }
                    else
                    {
                        this.QualifiedId = this.GetCommandElement(T.Tokens, 0);
                    }

                    
                    this.value = EnterRule.BuildStatments(pContext, pTokens.Tokens[2]);
                    if (!this.Context.AssignVariableCheck.ContainsKey(this.QualifiedId.ToLower()))
                    {
                        this.Context.AssignVariableCheck.Add(this.QualifiedId.ToLower(), this.QualifiedId.ToLower());
                    }
                    break;

            }
        }
        /// <summary>
        /// peforms an assign rule by assigning an expression to a variable.  return the variable that was assigned
        /// </summary>
        /// <returns>object</returns>
        public override object Execute()
        {
            object result = this.value.Execute();

            EpiInfo.Plugin.IVariable var;
            //DataType dataType = DataType.Unknown;
            string dataValue = string.Empty;
            var =  this.Context.CurrentScope.resolve(this.QualifiedId, this.Namespace);

            if (var != null)
            {
                if (var.VariableScope == EpiInfo.Plugin.VariableScope.DataSource)
                {
                    //IVariable fieldVar = new DataSourceVariableRedefined(var.Name, var.DataType);
                    //fieldVar.PromptText = var.PromptText;
                    var.Expression = result.ToString();
                    //var.DataType = 
                    //this.Context.CurrentScope.undefine(var.Name);
                    //this.Context.CurrentScope.define((EpiInfo.Plugin.IVariable) fieldVar);
                }
                else
                {
                    if (result != null)
                    {
                        var.Expression = result.ToString();
                    }
                    else
                    {
                        var.Expression = "Null";
                    }

                    if (var.VariableScope == EpiInfo.Plugin.VariableScope.Permanent)
                    {
                        //Rule_Context.UpdatePermanentVariable(var);
                    }
                }
            }
            else
            {
                if (result != null)
                {
                    EpiInfo.Plugin.IVariable v = this.Context.CurrentScope.resolve(this.QualifiedId);
                    if (result != null)
                    {
                        v.Expression = result.ToString();
                    }
                    else
                    {
                        v.Expression = "";
                    }
                    
                }
            }
            
            return result;
        }


        private EpiInfo.Plugin.DataType GuessDataTypeFromExpression(string expression)
        {
            double d = 0.0;
            DateTime dt;
            if (double.TryParse(expression, out d))
            {
                return DataType.Number;
            }
            if (System.Text.RegularExpressions.Regex.IsMatch(expression, "([+,-])"))
            {
                return DataType.Boolean;
            }
            if (DateTime.TryParse(expression, out dt))
            {
                return DataType.Date;
            }
            return DataType.Unknown;
        }



        public override void ToJavaScript(StringBuilder pJavaScriptBuilder)
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string uiSep = currentCulture.NumberFormat.NumberDecimalSeparator;
            string DateFormat = currentCulture.DateTimeFormat.ShortDatePattern;

            pJavaScriptBuilder.Append("cce_Context.setValue('");
            pJavaScriptBuilder.Append(this.QualifiedId.ToLower());
            pJavaScriptBuilder.Append("', ");
            this.value.ToJavaScript(pJavaScriptBuilder);
            pJavaScriptBuilder.Append(", '");
            pJavaScriptBuilder.Append(DateFormat + "'");
            pJavaScriptBuilder.Append(", '");
            pJavaScriptBuilder.Append(uiSep + "'");

            pJavaScriptBuilder.AppendLine(");");
        }


    }
}
