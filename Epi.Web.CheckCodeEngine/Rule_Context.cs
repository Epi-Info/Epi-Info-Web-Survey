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
        public Dictionary<string, List<string>> GroupVariableList;
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
        public System.Collections.Generic.Dictionary<string, EnterRule> OuterSubroutine;


        public System.Text.StringBuilder JavaScriptVariableDefinitions;


        public List<string> _HiddenFieldList;
        public List<string> _HighlightedFieldList;
        public List<string> _DisabledFieldList;
        public List<string> _RequiredFieldList;

        public string HiddenFieldList
        {
            get { 
                    StringBuilder result = new StringBuilder();
                    foreach(string s in _HiddenFieldList) 
                    {
                        result.Append(s);
                        result.Append(',');
                    }
                    return result.ToString();
                }
            set 
            {
                string[] list = value.Split(',');
                _HiddenFieldList.Clear();
                foreach (string s in list)
                {
                    _HiddenFieldList.Add(s.ToLower());
                }
            }
        }

        public string HighlightedFieldList
        {
            get { 
                    StringBuilder result = new StringBuilder();
                    foreach (string s in _HighlightedFieldList) 
                    {
                        result.Append(s);
                        result.Append(',');
                    }
                    return result.ToString();
                }
            set
            {
                string[] list = value.Split(',');
                _HighlightedFieldList.Clear();
                foreach (string s in list)
                {
                    _HighlightedFieldList.Add(s.ToLower());
                }
            }
        }

        public string DisabledFieldList
        {
            get { 
                    StringBuilder result = new StringBuilder();
                    foreach (string s in _DisabledFieldList) 
                    {
                        result.Append(s);
                        result.Append(',');
                    }
                    return result.ToString(); 
                }
            set
            {
                string[] list = value.Split(',');
                _DisabledFieldList.Clear();
                foreach (string s in list)
                {
                    _DisabledFieldList.Add(s.ToLower());
                }
            }
        }

        public string RequiredFieldList
        {
            get
            {
                StringBuilder result = new StringBuilder();
                foreach (string s in _RequiredFieldList)
                {
                    result.Append(s);
                    result.Append(',');
                }
                return result.ToString();
            }
            set
            {
                string[] list = value.Split(',');
                _RequiredFieldList.Clear();
                foreach (string s in list)
                {
                    _RequiredFieldList.Add(s.ToLower());
                }
            }
        }

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
            this.JavaScriptVariableDefinitions = new StringBuilder();
            this.DLLClassList = new Dictionary<string, IDLLClass>(StringComparer.OrdinalIgnoreCase);

            this._HiddenFieldList = new List<string>();
            this._HighlightedFieldList = new List<string>();
            this._DisabledFieldList = new List<string>();
            this._RequiredFieldList = new List<string>();

           //this.DefineVariablesCheckCode = new Rule_DefineVariables_Statement();
            this.GroupVariableList = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);


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
            this.OuterSubroutine = new Dictionary<string, EnterRule>(StringComparer.OrdinalIgnoreCase);


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


        public void LoadTemplate(XDocument pTemplateDoc, XDocument pSurveyResponseDoc)
        {

            // todo for each page in 
            var _FieldsTypeIDs = from _FieldTypeID in pTemplateDoc.Descendants("Field")
                                 select _FieldTypeID;

            string PageNumber = "";
            string defineFormat = "cce_Context.define(\"{0}\", \"{1}\", \"{2}\", \"{3}\");";
            string defineNumberFormat = "cce_Context.define(\"{0}\", \"{1}\", \"{2}\", new Number({3}));";


            foreach (var _FieldTypeID in _FieldsTypeIDs)
            {

                PluginVariable var = new PluginVariable();
                var.Name = _FieldTypeID.Attribute("Name").Value;
                var.VariableScope = VariableScope.DataSource;
                var.PageNumber = PageNumber;

                if (pSurveyResponseDoc != null)
                {
                    var.Expression = GetControlValue(pSurveyResponseDoc, var.Name);
                }


                switch (_FieldTypeID.Attribute("FieldTypeId").Value)
                {
                    case "1": // textbox
                        var.DataType = DataType.Text;
                        var.ControlType = "textbox";
                        //JavaScriptVariableDefinitions.AppendLine(string.Format(defineFormat, _FieldTypeID.Attribute("Name").Value, "textbox", "datasource", var.Expression)); 
                        break;

                    case "2"://Label/Title
                        var.DataType = DataType.Text;
                        var.ControlType = "label";
                        //continue;
                        break;
                    case "3"://UpperCase
                        var.DataType = DataType.Text;                      
                        var.ControlType = "textbox";                        
                        break;
                    case "4"://MultiLineTextBox
                        var.DataType = DataType.Text;
                        var.ControlType = "multiline";
                        //JavaScriptVariableDefinitions.AppendLine(string.Format(defineFormat, _FieldTypeID.Attribute("Name").Value, "multiline", "datasource", var.Expression)); 
                        break;
                    case "5"://NumericTextBox
                        var.DataType = DataType.Number;
                        var.ControlType = "numeric";
                        //JavaScriptVariableDefinitions.AppendLine(string.Format(defineNumberFormat, _FieldTypeID.Attribute("Name").Value, "number", "datasource", var.Expression)); 
                        break;
                    case "7":// 7 DatePicker
                        var.DataType = DataType.Date;
                        var.ControlType = "datepicker";
                        //JavaScriptVariableDefinitions.AppendLine(string.Format(defineFormat, _FieldTypeID.Attribute("Name").Value, "number", "datasource", var.Expression)); 
                        break;
                    case "8": //TimePicker
                        var.DataType = DataType.Time;
                        var.ControlType = "timepicker";

                        break;
                   
                    case "10"://CheckBox
                        var.DataType = DataType.Boolean;
                        var.ControlType = "checkbox";
                        //JavaScriptVariableDefinitions.AppendLine(string.Format(defineFormat, _FieldTypeID.Attribute("Name").Value, "checkbox", "datasource", var.Expression)); 
                        break;
                    case "11"://DropDown Yes/No
                        var.DataType = DataType.Boolean;
                        var.ControlType = "yesno";
                        //JavaScriptVariableDefinitions.AppendLine(string.Format(defineFormat, _FieldTypeID.Attribute("Name").Value, "yesno", "datasource", var.Expression)); 
                        break;
                    case "12"://RadioButton
                        var.DataType = DataType.Number;
                        var.ControlType = "radiobutton";
                        //JavaScriptVariableDefinitions.AppendLine(string.Format(defineFormat, _FieldTypeID.Attribute("Name").Value, "yesno", "datasource", var.Expression)); 
                        break;

                    case "13": //CommandButton
                        var.DataType = DataType.Text;
                        var.ControlType = "commandbutton";
                        break;
                    case "15"://MirrorField
                        var.DataType = DataType.Text;
                        var.ControlType = "mirrorfield";                       
                        break;

                    case "17"://DropDown LegalValues
                        var.DataType = DataType.Text;
                        var.ControlType = "legalvalues";
                        //JavaScriptVariableDefinitions.AppendLine(string.Format(defineFormat, _FieldTypeID.Attribute("Name").Value, "legalvalue", "datasource", var.Expression));
                        break;
                    case "18"://DropDown Codes
                        var.DataType = DataType.Text;
                        var.ControlType = "codes";
                        //JavaScriptVariableDefinitions.AppendLine(string.Format(defineFormat, _FieldTypeID.Attribute("Name").Value, "code", "datasource", var.Expression));
                        break;
                    case "19"://DropDown CommentLegal
                        var.DataType = DataType.Text;
                        var.ControlType = "commentlegal";
                        //JavaScriptVariableDefinitions.AppendLine(string.Format(defineFormat, _FieldTypeID.Attribute("Name").Value, "commentlegal", "datasource", var.Expression)); 
                        break;
                    case "20": //relatebutton
                        var.DataType = DataType.Text;
                        var.ControlType = "relatebutton";
                        break;
                    case "21"://GroupBox
                        var.DataType = DataType.Unknown;
                        var.ControlType = "groupbox";
                        var.Expression = _FieldTypeID.Attribute("List").Value;
                        //List="Otherpleasespecify,DoubleHearingProtectionWearingEarPlugsandMuffsatthesametime,FittedEarPlugs,EarMuffs,DisposableEarPlugs,FullFaceAirPurifyingCartridgeRespirator,FullFaceorHoodSuppliedAirRespirator,HalfFaceAirPurifyingCartridgeRespirator,DisposibleRespiratorDustMask,WeldingHelmetwithDarkFacePlate,ProtectiveLongSleeveJacket,HeatResistantandFlameRetardantClothing,HeavyLeatherGloves,InsulatedGloves,FaceShield,DarkGoggles,Goggles,None2"
                        string[] IdentifierList = var.Expression.Split(',');
                        string Identifier = _FieldTypeID.Attribute("Name").Value;
                        if (this.GroupVariableList.ContainsKey(Identifier))
                        {
                            this.GroupVariableList[Identifier].Clear();
                        }
                        else
                        {
                            this.GroupVariableList.Add(Identifier, new List<string>());
                        }

                        foreach (string s in IdentifierList)
                        {
                            this.GroupVariableList[Identifier].Add(s);
                            this.GroupVariableList[Identifier].Add(Identifier);
                        }

                        break;
                }
                this.DefineVariable(var);

            }
        }

        public static string GetControlValue(XDocument xdoc, string ControlName)
        {

            string ControlValue = "";

            var _ControlValues = from _ControlValue in
                                        xdoc.Descendants("ResponseDetail")
                                    where _ControlValue.Attribute("QuestionName").Value == ControlName.ToString()
                                    select _ControlValue;

            foreach (var _ControlValue in _ControlValues)
            {
                ControlValue = _ControlValue.Value;
            }
            ControlValue = ControlValue.Replace("\n", " ");

            return ControlValue;
        }


        public void GetVariableJavaScript(StringBuilder pJavaScriptBuilder)
        {
            string defineFormat = "cce_Context.define(\"{0}\", \"{1}\", \"{2}\", \"{3}\", \"{4}\");";
            string defineNumberFormat = "cce_Context.define(\"{0}\", \"{1}\", \"{2}\", \"{3}\", new Number({4}));";


            foreach (PluginVariable var in this.CurrentScope.FindVariables( VariableScope.DataSource | VariableScope.Global | VariableScope.Permanent | VariableScope.Standard))
            {
                string DataSource;
                switch (var.VariableScope)
                {
                    case VariableScope.DataSource:
                        DataSource = "datasource";
                        break;
                    case VariableScope.Global:
                        DataSource = "global";
                        break;
                    case VariableScope.Permanent:
                        DataSource = "permanent";
                        break;
                    case VariableScope.Standard:
                    default:
                        DataSource = "standard";
                        break;

                }

                switch (var.ControlType)
                {

                    case "checkbox":
                    case "yesno":
                        pJavaScriptBuilder.AppendLine(string.Format(defineFormat, var.Name, var.ControlType, DataSource, var.PageNumber, var.Expression));
                        break;

                    case "numeric":
                        pJavaScriptBuilder.AppendLine(string.Format(defineNumberFormat, var.Name, var.ControlType, DataSource, var.PageNumber, var.Expression));
                        break;
                    case "groupbox":
                        pJavaScriptBuilder.AppendLine(string.Format(defineFormat, var.Name, var.ControlType, DataSource, var.PageNumber, var.Expression));
                        break;
                    case "commentlegal":
                    case  "codes":
                    case "legalvalues":
                    case "datepicker":
                    case "timepicker":
                    case "multiline":
                         pJavaScriptBuilder.AppendLine(string.Format(defineFormat, var.Name, var.ControlType, DataSource, var.PageNumber, var.Expression.Replace("\n"," ")));
                        break;
                    case "textbox": 
                    case "commandbutton":                   

                    default:
                        pJavaScriptBuilder.AppendLine(string.Format(defineFormat, var.Name, var.ControlType, DataSource, var.PageNumber, var.Expression));
                        break;

                }

            }
        }

        public void ExpandGroupVariables(List<string> pIdentifierList, ref bool pIsExceptionList)
        {
            List<string> result = new List<string>();
            List<EpiInfo.Plugin.IVariable> List = this.Scope.FindVariables(VariableScope.DataSource | VariableScope.Standard);

            for (int i = 0; i < pIdentifierList.Count; i++)
            {
                pIdentifierList[i] = pIdentifierList[i].ToUpper();
            }

            if (pIdentifierList.Count == 1 && pIdentifierList[0] == "*")
            {
                pIdentifierList.Clear();

                foreach (DataColumn C in List)
                {
                    pIdentifierList.Add(C.ColumnName.ToUpper());
                }
            }


            if (pIsExceptionList)
            {
                List<string> List2 = new List<string>();


                foreach (DataColumn C in List)
                {
                    List2.Add(C.ColumnName.ToUpper());
                }


                foreach (string s in pIdentifierList)
                {
                    if (this.GroupVariableList.ContainsKey(s))
                    {
                        List<string> GroupVarList = this.GroupVariableList[s];
                        foreach (string Variable in GroupVarList)
                        {
                            if (List2.Contains(Variable.ToUpper()))
                            {
                                List2.Remove(Variable.ToUpper());
                            }
                        }
                    }
                    else
                    {
                        if (List2.Contains(s.ToUpper()))
                        {

                            List2.Remove(s.ToUpper());
                        }
                    }
                }
                pIdentifierList.Clear();

                foreach (string s in List2)
                {
                    pIdentifierList.Add(s);
                }


                pIsExceptionList = false;
            }
            else
            {
                foreach (KeyValuePair<string, List<string>> GroupVarList in this.GroupVariableList)
                {
                    if (pIdentifierList.Contains(GroupVarList.Key.ToUpper()))
                    {
                        pIdentifierList.Remove(GroupVarList.Key.ToUpper());

                        foreach (string Variable in GroupVarList.Value)
                        {
                            if (!pIdentifierList.Contains(Variable.ToUpper()))
                            {
                                pIdentifierList.Add(Variable.ToUpper());
                            }

                        }
                    }
                }
            }

        }

        public void GetSubroutineJavaScript(StringBuilder pJavaScriptBuilder)
        {
            foreach (KeyValuePair<string,EnterRule> kvp in this.OuterSubroutine)
            {
                kvp.Value.ToJavaScript(pJavaScriptBuilder);
            }
        }
    }
}
