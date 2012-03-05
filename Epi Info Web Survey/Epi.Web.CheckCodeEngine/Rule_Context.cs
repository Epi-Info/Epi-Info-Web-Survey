using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;
using com.calitha.goldparser;
using Epi;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

/*
using Epi.Collections;
using Epi.Data;
using Epi.Data.Services;
using Epi.Fields;
using VariableCollection = Epi.Collections.NamedObjectCollection<Epi.IVariable>;*/
using Epi.Core.EnterInterpreter.Rules;
using EpiInfo.Plugin;

namespace Epi.Core.EnterInterpreter
{
    public class Rule_Context : ICommandContext
    {
        public IEnterInterpreterHost EnterCheckCodeInterface;
        public StringBuilder ProgramText;
        /*public System.Collections.Specialized.NameValueCollection PermanantVariables;
public System.Collections.Specialized.NameValueCollection GlobalVariables;*/
        public System.Collections.Generic.Dictionary<string, string> StandardVariables;
        public System.Collections.Generic.Dictionary<string, string> AssignVariableCheck;
        //private IMemoryRegion memoryRegion;
        private IScope currentScope;

        public Rule_DefineVariables_Statement DefineVariablesCheckcode;
        public Rule_View_Checkcode_Statement View_Checkcode;
        public Rule_Record_Checkcode_Statement Record_Checkcode;

        public System.Collections.Generic.Dictionary<string, IDLLClass> DLLClassList;

        public System.Collections.Generic.Dictionary<string, EnterRule> Page_Checkcode;
        public System.Collections.Generic.Dictionary<string, EnterRule> Field_Checkcode;


        public System.Collections.Generic.Dictionary<string, EnterRule> BeforeCheckCode;
        public System.Collections.Generic.Dictionary<string, EnterRule> AfterCheckCode;
        public System.Collections.Generic.Dictionary<string, EnterRule> PageBeforeCheckCode;
        public System.Collections.Generic.Dictionary<string, EnterRule> PageAfterCheckCode;
        public System.Collections.Generic.Dictionary<string, EnterRule> FieldBeforeCheckCode;
        public System.Collections.Generic.Dictionary<string, EnterRule> FieldAfterCheckCode;
        public System.Collections.Generic.Dictionary<string, EnterRule> FieldClickCheckCode;
        public System.Collections.Generic.Dictionary<string, EnterRule> Subroutine;


        private string[] parseGetCommandSearchText(string pSearchText)
        {
            string[] result = null;

            string[] temp = pSearchText.Split('&');
            result = new string[temp.Length];
            for (int i = 0; i < temp.Length; i++)
            {
                result[i] = temp[i].Split('=')[1];
            }

            return result;
        }

        public IScope Scope { get { return this.currentScope; } }

        public ICommand GetCommand(string pSearchText)
        {
            ICommand result = null;
            string Level = null;
            string Event = null;
            string Identifier = null;

            //string SearchRule = null;

            string[] Parameters = parseGetCommandSearchText(pSearchText);
            Level = Parameters[0].ToLower();
            Event = Parameters[1].ToLower();
            Identifier = Parameters[2].ToLower();

            switch (Level.ToLower())
            {
                case "view":
                    if (Event.ToLower() == "")
                    {
                        result = this.View_Checkcode;
                    }
                    else
                    if (Event.ToLower() == "before")
                    {
                        if (this.BeforeCheckCode.ContainsKey("view"))
                        {
                            result = this.BeforeCheckCode["view"];
                        }
                    }
                    else if (Event.ToLower() == "after")
                    {
                        if (this.AfterCheckCode.ContainsKey("view"))
                        {
                            result = this.AfterCheckCode["view"];
                        }
                    }
                    break;
                case "record":
                    if (Event.ToLower() == "")
                    {
                        result = this.Record_Checkcode;
                    }
                    else
                    if (Event.ToLower() == "before")
                    {
                        if (this.BeforeCheckCode.ContainsKey("record"))
                        {
                            result = this.BeforeCheckCode["record"];
                        }
                    }
                    else if (Event.ToLower() == "after")
                    {
                        if (this.AfterCheckCode.ContainsKey("record"))
                        {
                            result = this.AfterCheckCode["record"];
                        }
                    }
                    break;
                case "page":
                    if (Event.ToLower() == "")
                    {
                        if (this.Page_Checkcode.ContainsKey(Identifier))
                        {
                            result = this.Page_Checkcode[Identifier];
                        }
                    }
                    else
                    if (Event.ToLower() == "before")
                    {
                        if (this.PageBeforeCheckCode.ContainsKey(Identifier))
                        {
                            result = this.PageBeforeCheckCode[Identifier];
                        }
                    }
                    else if (Event.ToLower() == "after")
                    {
                        if (this.PageAfterCheckCode.ContainsKey(Identifier))
                        {
                            result = this.PageAfterCheckCode[Identifier];
                        }
                    }
                    break;
                case "field":
                    if (Event.ToLower() == "")
                    {
                        if (this.Field_Checkcode.ContainsKey(Identifier))
                        {
                            result = this.Field_Checkcode[Identifier];
                        }
                    }
                    else
                    if (Event.ToLower() == "before")
                    {
                        if (this.FieldBeforeCheckCode.ContainsKey(Identifier))
                        {
                            result = this.FieldBeforeCheckCode[Identifier];
                        }
                    }
                    else if (Event.ToLower() == "after")
                    {
                        if (this.FieldAfterCheckCode.ContainsKey(Identifier))
                        {
                            result = this.FieldAfterCheckCode[Identifier];
                        }
                    }
                    else if (Event.ToLower() == "click")
                    {
                        if (this.FieldClickCheckCode.ContainsKey(Identifier))
                        {
                            result = this.FieldClickCheckCode[Identifier];
                        }
                    }
                    break;
                case "sub":
                    if (this.Subroutine.ContainsKey(Event))
                    {
                        result = this.Subroutine[Event];
                    }
                    break;
                case "definevariables":
                    result = this.DefineVariablesCheckcode;
                    break;
            }

            return result;
        }

        public IScope CurrentScope
        {
            get { return this.currentScope; }
        }
        /*
        public IMemoryRegion MemoryRegion
        {
            get
            {
                return this.memoryRegion;
            }

            set { this.memoryRegion = value; }
        }*/

        public Rule_Context()
        {
            this.currentScope = new cSymbolTable("global",null);
            //Rule_Context.LoadPermanentVariables(this.currentScope);
            this.Initialize();
        }


        public Rule_Context(IScope pScope)
        {
            this.currentScope = new cSymbolTable(pScope);
            this.Initialize();
        }


        private void Initialize()
        {
            //this.memoryRegion = new MemoryRegion();

            //this.currentScope = new cSymbolTable(null);

            this.ProgramText = new StringBuilder();
            this.DLLClassList = new Dictionary<string, IDLLClass>(StringComparer.OrdinalIgnoreCase);

           //this.DefineVariablesCheckCode = new Rule_DefineVariables_Statement();

            this.Page_Checkcode = new Dictionary<string, EnterRule>(StringComparer.OrdinalIgnoreCase);
            this.Field_Checkcode = new Dictionary<string, EnterRule>(StringComparer.OrdinalIgnoreCase);

            this.BeforeCheckCode = new Dictionary<string, EnterRule>(StringComparer.OrdinalIgnoreCase);
            this.AfterCheckCode = new Dictionary<string, EnterRule>(StringComparer.OrdinalIgnoreCase);
            this.PageBeforeCheckCode = new Dictionary<string, EnterRule>(StringComparer.OrdinalIgnoreCase);
            this.PageAfterCheckCode = new Dictionary<string, EnterRule>(StringComparer.OrdinalIgnoreCase);
            this.FieldBeforeCheckCode = new Dictionary<string, EnterRule>(StringComparer.OrdinalIgnoreCase);
            this.FieldAfterCheckCode = new Dictionary<string, EnterRule>(StringComparer.OrdinalIgnoreCase);
            this.FieldClickCheckCode = new Dictionary<string, EnterRule>(StringComparer.OrdinalIgnoreCase);
            this.Subroutine = new Dictionary<string, EnterRule>(StringComparer.OrdinalIgnoreCase);


        }

        /*
        public static void DeletePermanentVariable(string variableName)
        {

            Configuration config = Configuration.GetNewInstance();
            DataRow[] result = config.PermanentVariables.Select("Name='" + variableName + "'");
            if (result.Length != 1)
            {
                throw new ConfigurationException(ConfigurationException.ConfigurationIssue.ContentsInvalid);
            }
            result[0].Delete();
            Configuration.Save(config);
        }

        public static void UpdatePermanentVariable(EpiInfo.Plugin.IVariable variable)
        {

            Configuration config = Configuration.GetNewInstance();
            DataRow[] result = config.PermanentVariables.Select("Name='" + variable.Name + "'");
            if (result.Length < 1)
            {
                config.PermanentVariables.AddPermanentVariableRow(
                   variable.Name,
                   variable.Expression ?? "",
                    (int)variable.DataType,
                   config.ParentRowPermanentVariables);
            }
            else if (result.Length == 1)
            {
                ((DataSets.Config.PermanentVariableRow)result[0]).DataValue = variable.Expression ?? "";
                ((DataSets.Config.PermanentVariableRow)result[0]).DataType = (int)variable.DataType;
            }
            else
            {
                throw new ConfigurationException(ConfigurationException.ConfigurationIssue.ContentsInvalid, "Duplicate permanent variable rows encountered.");
            }

            Configuration.Save(config);
        }

        public static void LoadPermanentVariables(IScope pScope)
        {
            //lock (syncLock)
            //{
                
                Configuration config = Configuration.GetNewInstance();
                foreach (Epi.DataSets.Config.PermanentVariableRow row in config.PermanentVariables)
                {
                    EpiInfo.Plugin.IVariable var = new PluginVariable(row.Name, (EpiInfo.Plugin.DataType)row.DataType, VariableScope.Permanent, row.DataValue);
                    pScope.define(var);
                }
            //}
        }*/

        public object GetVariable(string name)
        {
            object result = null;
            //result = StandardVariables[name];
            result = this.currentScope.resolve(name);
            return result;
        }

        public bool SetVariable(string name, object setValue)//, Epi.VariableType pType)
        {
            bool result = false;
            string value = setValue.ToString();
            if (StandardVariables.ContainsKey(name))
            {
                StandardVariables[name] = value;
            }
            else
            {
                StandardVariables.Add(name, setValue.ToString());
            }

            return result;
        }


        /// <summary>
        /// Clears the session state
        /// </summary>
        public void ClearState()
        {
            this.currentScope.RemoveVariablesInScope(EpiInfo.Plugin.VariableScope.Standard, this.Scope.Name);
            //this.currentScope.RemoveVariablesInScope(EpiInfo.Plugin.VariableScope.Global);
            this.DefineVariablesCheckcode = null;
            this.View_Checkcode = null;
            this.Record_Checkcode = null;
            this.Page_Checkcode.Clear();
            this.Field_Checkcode.Clear();


            //this.MemoryRegion.RemoveVariablesInScope(VariableType.DataSource);
            //this.MemoryRegion.RemoveVariablesInScope(VariableType.DataSourceRedefined);

            //VariableCollection vars = module.Processor.GetVariablesInScope(VariableType.DataSource | VariableType.Standard | VariableType.DataSourceRedefined);
            //foreach (IVariable var in vars)
            //{
            //    module.Processor.UndefineVariable(var.Name);
            //}

            // Remove the current DataSourceInfo object and create a new one.
            this.BeforeCheckCode.Clear();
            this.AfterCheckCode.Clear();
            this.PageBeforeCheckCode.Clear();
            this.PageAfterCheckCode.Clear();
            this.FieldBeforeCheckCode.Clear();
            this.FieldAfterCheckCode.Clear();
            this.FieldClickCheckCode.Clear();
            this.Subroutine.Clear();
            //this.ProgramText.Length = 0;
        }


        public List<EpiInfo.Plugin.IVariable> GetVariablesInScope()
        {
            return this.currentScope.FindVariables(VariableScope.DataSource | VariableScope.DataSourceRedefined | VariableScope.Global | VariableScope.Permanent | VariableScope.Standard | VariableScope.System | VariableScope.Undefined);
        }

        public List<EpiInfo.Plugin.IVariable> GetVariablesInScope(VariableScope scopeCombination)
        {
            return this.currentScope.FindVariables(scopeCombination);
        }

        public bool TryGetVariable(string p, out EpiInfo.Plugin.IVariable var)
        {
            bool result = false;
            var = null;

            var = this.currentScope.resolve(p);

            if (var != null)
            {
                result = true;
            }
            return result;
        }


        public void RemoveVariablesInScope(VariableScope varTypes)
        {
            this.currentScope.RemoveVariablesInScope(varTypes);
        }


        public void DefineVariable(EpiInfo.Plugin.IVariable variable)
        {
            this.currentScope.define(variable);
        }

        public void UndefineVariable(string varName)
        {
            this.currentScope.undefine(varName);
        }

        /*
        public string GetCodeBlock(string pLevel, string pIdentifier)
        {
            // pLevel = "definevariables","view", "record", "page", "field", "subroutine"
            ICommand result = null;

            try
            {
                string SearchRule = null;

                switch (pLevel.ToLower())
                {
                    case "view":
                        SearchRule = "<View_Checkcode_Statement>";
                        break;
                    case "definevariables":
                        SearchRule = "<DefineVariables_Statement>";
                        break;
                    case "field":
                        SearchRule = "<Field_Checkcode_Statement>";
                        break;
                    case "record":
                        SearchRule = "<Record_Checkcode_Statement>";
                        break;
                    case "page":
                        SearchRule = "<Page_Checkcode_Statement>";
                        break;
                    case "subroutine":
                        SearchRule = "<Subroutine_Statement>";
                        break;
                    default:
                        return result.ToString();


                }


                result = this.GetCommand("<start>");

            }
            catch
            {

            }
            return result.ToString();
        }*/

        private NonterminalToken FindBlock(NonterminalToken pT, string pSearchRule, string pIdentifier)
        {
            NonterminalToken result = null;
            NonterminalToken currentToken = pT;

            while (currentToken != null)
            {
                if (currentToken.Rule.Lhs.ToString().ToLower() == pSearchRule.ToLower())
                {
                    switch (pSearchRule)
                    {
                        case "<Field_Checkcode_Statement>":
                        case "<Page_Checkcode_Statement>":
                        case "<Subroutine_Statement>":
                            if (currentToken.Tokens[1].ToString().ToLower() == pIdentifier.ToLower())
                            {
                                result = currentToken;
                                return result;
                            }
                            break;
                        default:
                            result = currentToken;
                            return result;
                    }
                }


                if (currentToken.Tokens[0] is NonterminalToken)
                {
                    switch (currentToken.Rule.Rhs[0].ToString())
                    {
                        case "<Statements>":
                            currentToken = (NonterminalToken)currentToken.Tokens[1];
                            break;
                        case "<Statement>":
                        default:
                            currentToken = (NonterminalToken)currentToken.Tokens[0];
                            break;
                    }
                }
                else
                {
                    break;
                }
            }
            return result;
        }

        public void CheckAssignedVariables()
        {
            foreach (System.Collections.Generic.KeyValuePair<string, string> i in this.AssignVariableCheck)
            {
                if (this.currentScope.resolve(i.Key) == null)
                {
                    //throw new Exception(string.Format("variable {0} assigned but NOT defined", i.Key));
                }
            }
        }

        public IScope GetNewScope(string pName, IScope pParent)
        {
            return new cSymbolTable(pName, pParent);
        }


        public void LoadTemplate(XDocument pTemplateText)
        {
            var _FieldsTypeIDs = from _FieldTypeID in pTemplateText.Descendants("Field")
                                 select _FieldTypeID;


            foreach (var _FieldTypeID in _FieldsTypeIDs)
            {
                //var Value = GetControlValue(SurveyAnswer, _FieldTypeID.Attribute("Name").Value);
                //JavaScript.Append(GetFormJavaScript(checkcode, form, _FieldTypeID.Attribute("Name").Value));
                switch (_FieldTypeID.Attribute("FieldTypeId").Value)
                {
                    case "1": // textbox

                        //var _TextBoxValue = Value;
                        //form.AddFields(GetTextBox(_FieldTypeID, _Width, _Height, SurveyAnswer, _TextBoxValue));
                        //                                             pName, pType, pSource
                        //VariableDefinitions.AppendLine(string.Format(defineFormat, _FieldTypeID.Attribute("Name").Value, "textbox", "datasource", Value));
                        break;

                    case "2"://Label/Title
                        //form.AddFields(GetLabel(_FieldTypeID, _Width, _Height, SurveyAnswer));
                        //                                             pName, pType, pSource
                        //VariableDefinitions.AppendLine(string.Format(defineFormat, _FieldTypeID.Attribute("Name").Value, "lable", "datasource",Value)); 
                        break;
                    case "3"://Label

                        break;
                    case "4"://MultiLineTextBox

                        //var _TextAreaValue = Value;
                        //form.AddFields(GetTextArea(_FieldTypeID, _Width, _Height, SurveyAnswer, _TextAreaValue));
                        //                                             pName, pType, pSource
                        //VariableDefinitions.AppendLine(string.Format(defineFormat, _FieldTypeID.Attribute("Name").Value, "multiline", "datasource", Value));
                        break;
                    case "5"://NumericTextBox

                        //var _NumericTextBoxValue = Value;
                        //form.AddFields(GetNumericTextBox(_FieldTypeID, _Width, _Height, SurveyAnswer, _NumericTextBoxValue));
                        //                                             pName, pType, pSource
                        //VariableDefinitions.AppendLine(string.Format(defineNumberFormat, _FieldTypeID.Attribute("Name").Value, "number", "datasource", Value));
                        break;
                    // 7 DatePicker
                    case "7"://NumericTextBox

                        //var _DatePickerValue = Value;
                        //form.AddFields(GetDatePicker(_FieldTypeID, _Width, _Height, SurveyAnswer, _DatePickerValue));
                        //                                             pName, pType, pSource
                        //VariableDefinitions.AppendLine(string.Format(defineNumberFormat, _FieldTypeID.Attribute("Name").Value, "number", "datasource", Value));
                        break;
                    case "10"://CheckBox

                        //var _CheckBoxValue = Value;
                        //form.AddFields(GetCheckBox(_FieldTypeID, _Width, _Height, SurveyAnswer, _CheckBoxValue));
                        //                                             pName, pType, pSource
                        //VariableDefinitions.AppendLine(string.Format(defineFormat, _FieldTypeID.Attribute("Name").Value, "checkbox", "datasource", Value));
                        break;

                    case "11"://DropDown Yes/No

                        //var _DropDownSelectedValueYN = Value;
                        //form.AddFields(GetDropDown(_FieldTypeID, _Width, _Height, SurveyAnswer, _DropDownSelectedValueYN, "Yes,No", 11));
                        //                                             pName, pType, pSource
                        //VariableDefinitions.AppendLine(string.Format(defineFormat, _FieldTypeID.Attribute("Name").Value, "yesno", "datasource", Value));

                        break;
                    case "17"://DropDown LegalValues

                        //string DropDownValues1 = "";
                        //DropDownValues1 = GetDropDownValues(xdoc, _FieldTypeID.Attribute("Name").Value, _FieldTypeID.Attribute("SourceTableName").Value);
                        //var _DropDownSelectedValue1 = Value;
                        //form.AddFields(GetDropDown(_FieldTypeID, _Width, _Height, SurveyAnswer, _DropDownSelectedValue1, DropDownValues1, 17));
                        //                                          pName, pType, pSource
                        //VariableDefinitions.AppendLine(string.Format(defineFormat, _FieldTypeID.Attribute("Name").Value, "legalvalue", "datasource", Value));

                        break;
                    case "18"://DropDown Codes

                        //string DropDownValues2 = "";
                        //DropDownValues2 = GetDropDownValues(xdoc, _FieldTypeID.Attribute("Name").Value, _FieldTypeID.Attribute("SourceTableName").Value);
                        //var _DropDownSelectedValue2 = Value;
                        //form.AddFields(GetDropDown(_FieldTypeID, _Width, _Height, SurveyAnswer, _DropDownSelectedValue2, DropDownValues2, 18));
                        ////                                             pName, pType, pSource
                        //VariableDefinitions.AppendLine(string.Format(defineFormat, _FieldTypeID.Attribute("Name").Value, "code", "datasource", Value));

                        break;
                    case "19"://DropDown CommentLegal

                        //string DropDownValues = "";
                        //DropDownValues = GetDropDownValues(xdoc, _FieldTypeID.Attribute("Name").Value, _FieldTypeID.Attribute("SourceTableName").Value);
                        //var _DropDownSelectedValue = Value;
                        //form.AddFields(GetDropDown(_FieldTypeID, _Width, _Height, SurveyAnswer, _DropDownSelectedValue, DropDownValues, 19));
                        ////                                             pName, pType, pSource
                        //VariableDefinitions.AppendLine(string.Format(defineFormat, _FieldTypeID.Attribute("Name").Value, "commentlegal", "datasource", Value));

                        break;
                    case "21"://GroupBox
                        //var _GroupBoxValue = GetControlValue(SurveyAnswer, _FieldTypeID.Attribute("UniqueId").Value);
                        //form.AddFields(GetGroupBox(_FieldTypeID, _Width, _Height, SurveyAnswer, _GroupBoxValue));
                        ////                                             pName, pType, pSource
                        //VariableDefinitions.AppendLine(string.Format(defineFormat, _FieldTypeID.Attribute("Name").Value, "", "datasource",Value)); 
                        break;
                }

            }
        }


    }
}
