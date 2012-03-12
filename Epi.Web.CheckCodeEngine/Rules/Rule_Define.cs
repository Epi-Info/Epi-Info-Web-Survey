using System;
using System.Collections.Generic;
using System.Text;
using com.calitha.goldparser;
using System.Data;
using EpiInfo.Plugin;
/*using Epi.Data;
using Epi.Data.Services;
using Epi.DataSets;*/

namespace Epi.Core.EnterInterpreter.Rules
{
    public class Rule_Define : EnterRule
    {
        string Identifier = null;
        //strings named to match grammar
        string Variable_Scope = null;
        string VariableTypeIndicator = null;
        string Define_Prompt = null;
        EnterRule Expression = null;


        private EpiInfo.Plugin.VariableScope GetVariableScopeIdByName(string name)
        {
            VariableScope result =  VariableScope.Undefined;

            return result;
            /*
            string Query = "Name='" + name + "'";
            DataRow[] rows = AppData.Instance.VariableScopesDataTable.Select(Query);
            if (rows.GetUpperBound(0) >= 0)
            {
                return (EpiInfo.Plugin.VariableScope)int.Parse(rows[0]["Id"].ToString());
            }
            else
            {
                return 0;       // Unknown
            }*/
        }

        public Rule_Define(Rule_Context pContext, NonterminalToken pToken) : base(pContext)
        {
            //DEFINE Identifier <Variable_Scope> <VariableTypeIndicator> <Define_Prompt>
            //DEFINE Identifier '=' <Expression>

            Identifier = GetCommandElement(pToken.Tokens, 1);
            if (GetCommandElement(pToken.Tokens, 2) == "=")
            {
                this.Expression = EnterRule.BuildStatments(pContext, pToken.Tokens[3]);
                // set some defaults
                Variable_Scope = "STANDARD";
                VariableTypeIndicator  =  "";
                Define_Prompt = "";
            }
            else
            {
                Variable_Scope = GetCommandElement(pToken.Tokens, 2);//STANDARD | GLOBAL | PERMANENT |!NULL

                VariableTypeIndicator = GetCommandElement(pToken.Tokens, 3);
                Define_Prompt = GetCommandElement(pToken.Tokens, 4);
            }

        }


                /// <summary>
        /// peforms the Define rule uses the MemoryRegion and this.Context.DataSet to hold variable definitions
        /// </summary>
        /// <returns>object</returns>
        public override object Execute()
        {
            PluginVariable result = new PluginVariable();
            try
            {
                result.Name = this.Identifier;
                string dataTypeName = VariableTypeIndicator.Trim().ToUpper();
                EpiInfo.Plugin.DataType type = GetDataType(dataTypeName);
                string variableScope = Variable_Scope.Trim().ToUpper();
                EpiInfo.Plugin.VariableScope vt = EpiInfo.Plugin.VariableScope.Standard;

                if (!string.IsNullOrEmpty(variableScope))
                {
                    vt = this.GetVariableScopeIdByName(variableScope);
                }

                result.VariableScope = vt;
                result.DataType = type;
                result.ControlType = "hidden";
                this.Context.CurrentScope.define(result);

                return result;
            }
            catch (Exception ex)
            {
                //Epi.Diagnostics.Debugger.Break();
                //Epi.Diagnostics.Debugger.LogException(ex);
                throw ex;
            }
        }




        ///// <summary>
        ///// peforms the Define rule uses the MemoryRegion and this.Context.DataSet to hold variable definitions
        ///// </summary>
        ///// <returns>object</returns>
        //public override object Execute_Old()
        //{
        //    try
        //    {
        //        EpiInfo.Plugin.IVariable var = null;

        //        var = this.Context.CurrentScope.resolve(Identifier);
        //        if (var != null)
        //        {
        //            if (var.VariableScope != VariableScope.Permanent && var.VariableScope != VariableScope.Global)
        //                {
        //                    this.Context.EnterCheckCodeInterface.Dialog("Duplicate variable: " + Identifier, "Define");
        //                    return null;
        //                }
        //        }
                

        //        string dataTypeName = VariableTypeIndicator.Trim().ToUpper();
        //        EpiInfo.Plugin.DataType type = GetDataType(dataTypeName);
        //        string variableScope = Variable_Scope.Trim().ToUpper();
        //        EpiInfo.Plugin.VariableScope vt = EpiInfo.Plugin.VariableScope.Standard;
                
        //       //if(variableScope.Equals("PERMANENT", StringComparison.OrdinalIgnoreCase))
        //       //{
        //       //    vt = EpiInfo.Plugin.VariableScope.Permanent;
        //       //}
        //       //else if(variableScope.Equals("GLOBAL", StringComparison.OrdinalIgnoreCase))
        //       //{
        //       //     vt = EpiInfo.Plugin.VariableScope.Global;
        //       //}
        //       //else
        //       //{
        //       //     vt = EpiInfo.Plugin.VariableScope.Standard;
        //       //}

                
        //        if (!string.IsNullOrEmpty(variableScope))
        //        {
        //            vt = this.GetVariableScopeIdByName(variableScope);
        //        }

        //        //var = new PluginVariable(Identifier, type, vt, null);
        //        var = null;
        //        string promptString = Define_Prompt.Trim().Replace("\"", string.Empty);
        //        if (!string.IsNullOrEmpty(promptString))
        //        {
        //            promptString = promptString.Replace("(", string.Empty).Replace(")", string.Empty);
        //            promptString.Replace("\"", string.Empty);
        //        }
        //        //var.PromptText = promptString;
        //        //this.Context.MemoryRegion.DefineVariable(var);
        //        EpiInfo.Plugin.IVariable temp = (EpiInfo.Plugin.IVariable)var;
        //        this.Context.CurrentScope.define(temp);

        //        return var;
        //    }
        //    catch (Exception ex)
        //    {
        //        //Epi.Diagnostics.Debugger.Break();
        //        //Epi.Diagnostics.Debugger.LogException(ex);
        //        throw ex;
        //    }
        //}


        public override void ToJavaScript(StringBuilder pJavaScriptBuilder)
        {
            string defineFormat = "cce_Context.define(\"{0}\", \"{1}\", \"{2}\", \"{3}\");";
            string defineNumberFormat = "cce_Context.define(\"{0}\", \"{1}\", \"{2}\", new Number({3}));";

            PluginVariable var = (PluginVariable) this.Context.CurrentScope.resolve(this.Identifier);

            if (var == null)
            {
                //foreach (PluginVariable var in this.Context.CurrentScope.FindVariables( VariableScope.DataSource | VariableScope.Global | VariableScope.Permanent))
                var = new PluginVariable();

                var.Name = this.Identifier;
                string dataTypeName = VariableTypeIndicator.Trim().ToUpper();
                EpiInfo.Plugin.DataType type = GetDataType(dataTypeName);
                string variableScope = Variable_Scope.Trim().ToUpper();
                EpiInfo.Plugin.VariableScope vt = EpiInfo.Plugin.VariableScope.Standard;

                if (!string.IsNullOrEmpty(variableScope))
                {
                    vt = this.GetVariableScopeIdByName(variableScope);
                }

                var.VariableScope = vt;
                var.DataType = type;

            }
            switch (var.ControlType)
            {

                case "checkbox":
                case "yesno":
                    pJavaScriptBuilder.AppendLine(string.Format(defineFormat, var.Name, var.ControlType, "datasource", var.Expression));
                    break;

                case "numeric":
                    pJavaScriptBuilder.AppendLine(string.Format(defineNumberFormat, var.Name, var.ControlType, "datasource", var.Expression));
                    break;
                case "commentlegal":
                case  "codes":
                case "legalvalues":
                case "datepicker":
                case "multiline":
                case "textbox": 
                default:
                    pJavaScriptBuilder.AppendLine(string.Format(defineFormat, var.Name, var.ControlType, "datasource", var.Expression));
                    break;
            }
        }
    }
}
